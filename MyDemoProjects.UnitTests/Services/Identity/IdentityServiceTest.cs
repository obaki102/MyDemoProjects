using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using MyDemoProjects.Application.Infastructure.Data;
using MyDemoProjects.Application.Infastructure.Identity.Models;
using MyDemoProjects.Application.Infastructure.Identity.Services;
using MyDemoProjectsUnitTests.Services.Identity;
using System.Numerics;
using Xunit.Abstractions;
using static Humanizer.In;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace MyDemoProjectsUnitTests.Services.Identity;

public class IdentityServiceFixture : IDisposable
{

    public readonly IdentityService _identityService;
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
        fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x =>
                x.ChangeEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());
        fakeUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(
                        (string email) =>
                        email == "test@test.com" ?
                        new ApplicationUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = email,
                            UserName = email,
                            IsActive = true,
                            DisplayName = "Test",
                        } : null);
        fakeUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(
                        (ApplicationUser user, string password) =>
                         user.Email == "test@test.com" && password == "validPassword" ? true : false
                        );
        _userManager = fakeUserManager.Object;
        _roleManager = Mock.Of<FakeRoleManager>();

        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();
        configurationSection.Setup(a => a.Value).Returns("testestestestsestokenKey");
        configuration.Setup(a => a.GetSection("tokenKey")).Returns(configurationSection.Object);

        _identityService = new IdentityService(_userManager, _roleManager, configuration.Object);
    }

    public void Dispose()
    {
        _userManager?.Dispose();
        _roleManager?.Dispose();
    }
}

public class IdentityServiceTest : IClassFixture<IdentityServiceFixture>
{
    IdentityServiceFixture _identityServiceFixture;
    private readonly ITestOutputHelper _output;
    public IdentityServiceTest(IdentityServiceFixture identityServiceFixture, ITestOutputHelper output)
    {
        _identityServiceFixture = identityServiceFixture;
        _output = output;
    }

    [Fact]
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
        Assert.True(result.Messages[0].Equals("Please check your username and password."));

    }


    [Fact]
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
}
