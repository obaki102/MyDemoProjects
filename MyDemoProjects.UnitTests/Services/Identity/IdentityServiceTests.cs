using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Infastructure.Identity.Models;
using MyDemoProjects.Application.Infastructure.Identity.Services;
using MyDemoProjects.Application.Shared.Constants;
using System.Security.Claims;
using Xunit.Abstractions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using MyDemoProjects.UnitTests;

namespace MyDemoProjectsUnitTests.Services.Identity;
public class IdentityServiceFixture : IDisposable
{
    public readonly IIdentityService _identityService;
    public readonly UserManager<ApplicationUser> _userManager;
    public readonly RoleManager<ApplicationRole> _roleManager;
    public IdentityServiceFixture()
    {
        var users = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                UserName = "Test",
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.com"
            }
        }.AsQueryable();

        var fakeUserManager = new Mock<FakeUserManager>();
        fakeUserManager.Setup(x => x.Users)
            .Returns(users);
        fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x =>
                x.ChangeEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.AddLoginAsync(It.IsAny<ApplicationUser>(), It.IsAny<UserLoginInfo>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());
        fakeUserManager.Setup(x => x.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.FindByEmailAsync(It.Is<string>(s => s == "test@test.com")))
                .ReturnsAsync(
                        new ApplicationUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = "test@test.com",
                            UserName = "test@test.com",
                            IsActive = true,
                            DisplayName = "Test",
                        });
        fakeUserManager.Setup(x => x.CheckPasswordAsync(It.Is<ApplicationUser>(a => a.Email == "test@test.com"), It.Is<string>(p => p == "validPassword")))
                        .ReturnsAsync(true);
        _userManager = fakeUserManager.Object;
        _roleManager = Mock.Of<FakeRoleManager>();

        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();
        configurationSection.Setup(a => a.Value).Returns(Constants.TokenKey);
        configuration.Setup(a => a.GetSection("tokenKey")).Returns(configurationSection.Object);

        _identityService = new IdentityService(_userManager, _roleManager, configuration.Object);
    }

    public void Dispose()
    {
        _userManager?.Dispose();
        _roleManager?.Dispose();
    }
}

public class IdentityServiceTests : IClassFixture<IdentityServiceFixture>
{
    public readonly IdentityServiceFixture _identityServiceFixture = new();
    public readonly ITestOutputHelper _output;
    public IdentityServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }
    #region LoginUserAsync
    [Fact]
    [Trait("IdentityServiceTest", "LoginUserAsync")]
    public async Task LoginUserAsync_InValidCredentials_ShouldReturnFalse()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string dummyEmail = "test1@test";
        string dummyPasssword = "dummyPassword";

        //Act
        var result = await identityService.LoginUserAsync(dummyEmail, dummyPasssword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);
        //Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    [Trait("IdentityServiceTest", "LoginUserAsync")]
    public async Task LoginUserAsync_NoUserFound_ShouldReturnAnErrorMessage()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string dummyEmail = "test1@testsds";
        string dummyPasssword = "dummyPassword";

        //Act
        var result = await identityService.LoginUserAsync(dummyEmail, dummyPasssword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.NotEmpty(result.Messages[0]);
        Assert.True(result.Messages[0].Equals("Please check your username and password."));

    }

    [Fact]
    [Trait("IdentityServiceTest", "LoginUserAsync")]
    public async Task LoginUserAsync_WrongPassword_ShouldReturnAnErrorMessage()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string dummyEmail = "test@test.com";
        string dummyPasssword = "dummyPassword";

        //Act
        var result = await identityService.LoginUserAsync(dummyEmail, dummyPasssword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.NotEmpty(result.Messages[0]);
        Assert.True(result.Messages[0].Equals("Invalid Credentials."));

    }

    [Fact]
    [Trait("IdentityServiceTest", "LoginUserAsync")]
    public async Task LoginUserAsync_ValidCredentials_ShouldReturnTrue()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string dummyEmail = "test@test.com";
        string dummyPasssword = "validPassword";

        //Act
        var result = await identityService.LoginUserAsync(dummyEmail, dummyPasssword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.IsSuccess);
    }
    #endregion

    #region LoginExternalUserAsync
    [Fact]
    [Trait("IdentityServiceTest", "LoginExternalUserAsync")]
    public async Task LoginExternalUserAsync_ValidCredentials_ShouldReturnTrue()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        var dummyExternalUser = new LoginExternalUser
        {
            AccessToken = "dummyToken",
            EmailAddress = "test@test.com",
            PictureUrl = String.Empty,
            Provider = "Google",
            UserName = "test@test.com"

        };

        //Act
        var result = await identityService.LoginExternalUserAsync(dummyExternalUser);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    [Trait("IdentityServiceTest", "LoginExternalUserAsync")]
    public async Task LoginExternalUserAsync_InValidCredentials_CreateNewAccountAndShouldReturnTrue()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        var dummyExternalUser = new LoginExternalUser
        {
            AccessToken = "dummyToken",
            EmailAddress = "test1@test.com",
            PictureUrl = String.Empty,
            Provider = "Google",
            UserName = "test1@test.com"

        };

        //Act
        var result = await identityService.LoginExternalUserAsync(dummyExternalUser);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.IsSuccess);
    }
    #endregion

    #region CreateUserAsync
    [Fact]
    [Trait("IdentityServiceTest", "CreateUserAsync")]
    public async Task CreateUserAsync_NewUser_ShouldReturnTrue()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        var dummyUserName = new ApplicationUser
        {
            Email = "test2@test.com",
            UserName = "test2@test.com"

        };
        string dummyPassword = "dummyPasword123";

        //Act
        var result = await identityService.CreateUserAsync(dummyUserName, dummyPassword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    [Trait("IdentityServiceTest", "CreateUserAsync")]
    public async Task CreateUserAsync_ExistingUser_ShouldReturnFalse()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        var dummyUserName = new ApplicationUser
        {
            Email = "test@test.com",
            UserName = "test@test.com"

        };
        string dummyPassword = "dummyPasword123";

        //Act
        var result = await identityService.CreateUserAsync(dummyUserName, dummyPassword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.Messages[0].Equals("User already exist"));
        Assert.False(result.IsSuccess);
    }

    [Fact]
    [Trait("IdentityServiceTest", "CreateUserAsync")]
    public async Task CreateUserAsync_ExistingUser_ShouldReturnErrorMessage()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        var dummyUserName = new ApplicationUser
        {
            Email = "test@test.com",
            UserName = "test@test.com"

        };
        string dummyPassword = "dummyPasword123";

        //Act
        var result = await identityService.CreateUserAsync(dummyUserName, dummyPassword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.Messages[0].Equals("User already exist"));
    }
    #endregion

    #region ChangePasswordAsync
    [Fact]
    [Trait("IdentityServiceTest", "ChangePasswordAsync")]
    public async Task ChangePasswordAsync_InvalidUser_ShouldReturnFalse()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string dummyEmail = "test1@test.com";
        string dummyCurrentPassword = "dummyPasword123";
        string dummyNewPassword = "dummyPasword123";

        //Act
        var result = await identityService.ChangePasswordAsync(dummyEmail, dummyCurrentPassword, dummyNewPassword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    [Trait("IdentityServiceTest", "ChangePasswordAsync")]
    public async Task ChangePasswordAsync_InvalidUser_ShouldReturnErrorMessage()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string dummyEmail = "test1@test.com";
        string dummyCurrentPassword = "dummyPaswerd123";
        string dummyNewPassword = "dummyPasword123";

        //Act
        var result = await identityService.ChangePasswordAsync(dummyEmail, dummyCurrentPassword, dummyNewPassword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.Messages[0].Equals("User not found.Please check your username and password."));
    }

    [Fact]
    [Trait("IdentityServiceTest", "ChangePasswordAsync")]
    public async Task ChangePasswordAsync_ValidUser_ShouldReturnTrue()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string dummyEmail = "test@test.com";
        string dummyCurrentPassword = "dummyPaswerd123";
        string dummyNewPassword = "dummyPasword123";

        //Act
        var result = await identityService.ChangePasswordAsync(dummyEmail, dummyCurrentPassword, dummyNewPassword);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.IsSuccess);
    }
    #endregion

    #region ValidateTokenAndGetClaimsPrincipal
    [Fact]
    [Trait("IdentityServiceTest", "ValidateTokenAndGetClaimsPrincipal")]
    public void ValidateTokenAndGetClaimsPrincipal_ValidToken_ShouldReturnTrue()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        var dummyClaimsIdentity = new ClaimsIdentity(AppSecrets.Bearer);
        dummyClaimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "testDummy"));
        dummyClaimsIdentity.AddClaim(new(ApplicationClaimTypes.Status, "Active"));
        dummyClaimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.Email,"test@test.com")
            });
        dummyClaimsIdentity.AddClaims(new[] {
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddMinutes(30).ToShortTimeString()) });
        string validDummyToken = HelperMethods.GenerateDummyToken(dummyClaimsIdentity);
        _output.WriteLine($"DummyToken:{validDummyToken}");
        //Act
        var result = identityService.ValidateTokenAndGetClaimsPrincipal(validDummyToken);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    [Trait("IdentityServiceTest", "ValidateTokenAndGetClaimsPrincipal")]
    public void ValidateTokenAndGetClaimsPrincipal_InValidToken_ShouldReturnFalse()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string inValidDummyToken = "InvalidToken";
        //Act
        var result = identityService.ValidateTokenAndGetClaimsPrincipal(inValidDummyToken);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    [Trait("IdentityServiceTest", "ValidateTokenAndGetClaimsPrincipal")]
    public void ValidateTokenAndGetClaimsPrincipal_InValidToken_ShouldReturnErrorMessage()
    {
        //Arrange
        var identityService = _identityServiceFixture._identityService;
        string inValidDummyToken = "InvalidToken";
        //Act
        var result = identityService.ValidateTokenAndGetClaimsPrincipal(inValidDummyToken);
        foreach (var msg in result.Messages)
            _output.WriteLine(msg);

        //Assert
        Assert.True(result.Messages[0].Equals("Invalid token."));
    }
    #endregion
}

