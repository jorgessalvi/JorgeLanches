using Humanizer;
using JorgeLanches.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JorgeLanches.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }
        

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : " +
                DateTime.Now.ToString();
                
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody]UsuarioDTO model)
        {

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)             
                return BadRequest(result.Errors);            

            await _signInManager.SignInAsync(user, false);
            return Ok(GeraToken(model));

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido");
                return BadRequest();
            }

        }

        private UsuarioTokenDTO GeraToken(UsuarioDTO userInfo)
        {
            //define declaraçoes do usuario
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("boh", "f1"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //gera uma chave com base em um algoritmo simetrico            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            //Define Token expiration
            var expirationConfig = _config["TokenConfiguration:ExpireHours"];
            var tokenExpiration = DateTime.UtcNow.AddHours(double.Parse(expirationConfig));
            

            //Gerar Token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["TokenConfiguration:Issuer"],
                audience: _config["TokenConfiguration:Audience"],
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credenciais) ;

            return new UsuarioTokenDTO()
            {
                Authenticated= true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationDate= tokenExpiration,
                Message= "tOKen"


            };
        }


    }
}
