using JorgeLanches.DTOs;
using JorgeLanches.Models;
using JorgeLanches.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JorgeLanches.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityMinutes"],
                                                out int refreshTokenValidityMinutes);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireDate = DateTime.UtcNow.AddMinutes(refreshTokenValidityMinutes);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });

            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModelDTO model)
        {          
            if (await _userManager.FindByNameAsync(model.UserName!) != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseDTO { Status = "Error", Message = "Usuário já existe" });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                             new ResponseDTO { Status = "Error", Message = "Falha na criação do usuário" });
            }

            await _userManager.AddToRoleAsync(user, "User"); 

            return Ok(new ResponseDTO { Status = "Success", Message = "Usuário criado com sucesso" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModelDTO tokenModel)
        {
            if (tokenModel is null)
                return BadRequest("Request Inválido");

            string? accessToken = tokenModel.AccessToken ??
                                    throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken ??
                                    throw new ArgumentNullException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken, _configuration);

            if (principal is null)
                return BadRequest("Token Inválido");

            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpireDate <= DateTime.UtcNow)
                return BadRequest("Token Inválido");
            

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize(Policy = "MasterAdminOnly")]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                return BadRequest("invalid Username");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [Authorize(Policy = "MasterAdminOnly")]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);            
            if (!roleExists)
            {
                var roleCreateResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleCreateResult.Succeeded)
                {
                    return Ok(new ResponseDTO { Status = "Success", Message = $"Role {roleName} adicionada." });
                }
                else
                {
                    return BadRequest(new ResponseDTO { Status = "Error", Message = $"Problema ao Adicionar a Role {roleName}." });
                }
            }
            return BadRequest(new ResponseDTO { Status = "Error", Message = $"Role {roleName} já existe." });          
        }

        [Authorize(Policy = "MasterAdminOnly")]
        [HttpPost]
        [Route("BindUserToRole")]
        public async Task<IActionResult> BindUserToRole(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
                return BadRequest(new ResponseDTO { Status = "Error", Message = $"User {userName} Não encntrado." });

            var roleBindResult = await _userManager.AddToRoleAsync(user, roleName);

            if (roleBindResult.Succeeded)
            {
                return Ok(new ResponseDTO { Status = "Success", Message = $"User {userName} agora possui a role {roleName}." });
            }
            else
            {
                return BadRequest(new ResponseDTO { Status = "Error", Message = $"Problema ao Adicionar a Role {roleName} ao user {userName}." });
            }
        }
    }
}
