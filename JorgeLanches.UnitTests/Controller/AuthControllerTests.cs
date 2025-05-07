using JorgeLanches.Controllers;
using JorgeLanches.DTOs;
using JorgeLanches.Models;
using JorgeLanches.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Moq;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JorgeLanches.UnitTests;

public class AuthControllerTests
{
    private Mock<ApplicationUser> _applicationUser;
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private Mock<IConfiguration> _configurationMock;
    private AuthController _authController;

    [SetUp]
    public void Setup()
    {
        _applicationUser = new Mock<ApplicationUser>();
        _tokenServiceMock = new Mock<ITokenService>();

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                                new Mock<IUserStore<ApplicationUser>>().Object,
                                new Mock<IOptions<IdentityOptions>>().Object,
                                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                                new IUserValidator<ApplicationUser>[0],
                                new IPasswordValidator<ApplicationUser>[0],
                                new Mock<ILookupNormalizer>().Object,
                                new Mock<IdentityErrorDescriber>().Object,
                                new Mock<IServiceProvider>().Object,
                                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                                new Mock<IRoleStore<IdentityRole>>().Object,
                                new IRoleValidator<IdentityRole>[0],
                                new Mock<ILookupNormalizer>().Object,
                                new Mock<IdentityErrorDescriber>().Object,
                                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);


        _configurationMock = new Mock<IConfiguration>();

        _authController = new AuthController(_tokenServiceMock.Object, _userManagerMock.Object, _roleManagerMock.Object, _configurationMock.Object);

        
    }

    [Test]
    public async Task Login_InvalidLogin_ReturnUnauthorized()
    {
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => null);

        var result = await _authController.Login(new LoginModelDTO());
        var statusResult = result as UnauthorizedResult;

        Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
    }

    [Test]
    public async Task Login_InvalidPassword_ReturnUnauthorized()
    {
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => new ApplicationUser());
        _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(),It.IsAny<String>())).ReturnsAsync(() => false);

        var result = await _authController.Login(new LoginModelDTO());
        var statusResult = result as UnauthorizedResult;

        Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
    }

    [Test]
    public async Task Login_ValidLogin_ReturnOk()
    {
        var user = new ApplicationUser { Email = "abc@123.com", UserName = "user" };
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<String>())).ReturnsAsync(() => true);
        _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(() => new List<String>());
        _tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>(), _configurationMock.Object)).Returns(new JwtSecurityToken());

        var result = await _authController.Login(new LoginModelDTO());
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(statusResult.Value.ToString(), Does.Contain("Token"));
        Assert.That(statusResult.Value.ToString(), Does.Contain("RefreshToken"));
        Assert.That(statusResult.Value.ToString(), Does.Contain("RefreshToken"));
    }

    [Test]
    public async Task Register_UserAlreadyExists_ReturnInternalError()
    {
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => new ApplicationUser());

        var result = await _authController.Register(new RegisterModelDTO { UserName = "abc",});
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<ObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public async Task Register_CreateFails_ReturnInternalError()
    {

        _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<String>())).ReturnsAsync(() => new IdentityResult());

        var result = await _authController.Register(new RegisterModelDTO { UserName = "abc", Email = "123" });
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<ObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }

    [Test]
    public async Task Register_Success_ReturnOk()
    {
        var identityResult = new IdentityResult();
        identityResult = IdentityResult.Success;
        _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<String>())).ReturnsAsync(() => identityResult);

        var result = await _authController.Register(new RegisterModelDTO { UserName = "abc", Email = "123" });
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }

    [Test]
    public async Task RefreshToken_InvalidToken_ReturnBadRequest()
    {
        var result = await _authController.RefreshToken((TokenModelDTO)null);
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task RefreshToken_NullPrincipal_ReturnBadRequest()
    {
        _tokenServiceMock.Setup(s => s.GetPrincipalFromExpiredToken(It.IsAny<String>(), _configurationMock.Object)).Returns(() => null);
        var tokenDTO = new TokenModelDTO { AccessToken = "", RefreshToken = "" };

        var result = await _authController.RefreshToken(tokenDTO);
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task RefreshToken_InvalidUser_ReturnBadRequest()
    {
        var claismMock = new Mock<ClaimsPrincipal>();        
        var tokenDTO = new TokenModelDTO { AccessToken = "", RefreshToken = "" };
        claismMock.Setup(c => c.Identity.Name).Returns("");
        _tokenServiceMock.Setup(s => s.GetPrincipalFromExpiredToken(It.IsAny<String>(), _configurationMock.Object)).Returns(() => claismMock.Object );        

        var result = await _authController.RefreshToken(tokenDTO);
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task RefreshToken_Success_ReturnObjectResultWithNewToken()
    {
        var user = new ApplicationUser { UserName = "",
                                         RefreshToken = "",
                                         RefreshTokenExpireDate = DateTime.UtcNow.AddMinutes(1) };
        var claismMock = new Mock<ClaimsPrincipal>();
        var tokenDTO = new TokenModelDTO { AccessToken = "", RefreshToken = "" };
        claismMock.Setup(c => c.Identity.Name).Returns("");
        _tokenServiceMock.Setup(s => s.GetPrincipalFromExpiredToken(It.IsAny<String>(), _configurationMock.Object)).Returns(() => claismMock.Object);
        _tokenServiceMock.Setup(s => s.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>(), _configurationMock.Object)).Returns(new JwtSecurityToken());
        _tokenServiceMock.Setup(s => s.GenerateRefreshToken()).Returns("");
        _userManagerMock.Setup(m => m.FindByNameAsync("")).ReturnsAsync(() => user);
        

        var result = await _authController.RefreshToken(tokenDTO);
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<ObjectResult>());
        Assert.That(statusResult.Value is not null);
    }

    [Test]
    public async Task Revoke_InvalidUser_ReturnBadRequest()
    {
        var result = await _authController.Revoke("");
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task Revoke_Success_ReturnNoContent()
    {
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => new ApplicationUser { RefreshToken = ""});

        var result = await _authController.Revoke("");
        var statusResult = result as NoContentResult;

        Assert.That(result, Is.TypeOf<NoContentResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }

    [Test]
    public async Task CreateRole_RoleAlreadyExists_ReturnBadRequest()
    {
        _roleManagerMock.Setup(r => r.RoleExistsAsync("")).ReturnsAsync(() => true);

        var result = await _authController.CreateRole("");
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }

    [Test]
    public async Task CreateRole_RoleManagerFailsToCreate_ReturnBadRequest()
    {
        var identity = new IdentityResult();        
        _roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(identity);

        var result = await _authController.CreateRole("");
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }

    [Test]
    public async Task CreateRole_Success_ReturnOk()
    {
        var identity = new IdentityResult();
        identity = IdentityResult.Success;
        _roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(identity);

        var result = await _authController.CreateRole("");
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }

    [Test]
    public async Task BindUserToRole_InvalidUser_ReturnBadRequest()
    {
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => null);

        var result = await _authController.BindUserToRole("", "");
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }

    [Test]
    public async Task BindUserToRole_UserManagerFailsToInclude_ReturnBadRequest()
    {
        var identity = new IdentityResult();
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => new ApplicationUser());
        _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "")).ReturnsAsync(() => identity);

        var result = await _authController.BindUserToRole("", "");
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }

    [Test]
    public async Task BindUserToRole_Success_ReturnOk()
    {
        var identity = new IdentityResult();
        identity = IdentityResult.Success;
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(() => new ApplicationUser());
        _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "")).ReturnsAsync(() => identity);

        var result = await _authController.BindUserToRole("", "");
        var statusResult = result as ObjectResult;

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(statusResult.Value, Is.TypeOf<ResponseDTO>());
    }
}
