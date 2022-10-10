using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Infastructure.Identity.Services;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.UnitTests.Features
{
    public class AuthenticationTests
    {

        #region LoginUser
        [Fact]
        [Trait("AuthenticationTests", "LoginUser")]
        public async Task LoginUser_ValidCredentials_ShouldReturnTrue()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginUserAsync(It.Is<string>(s => s == "test@test.com"), It.Is<string>(p => p == "ValidPassword")))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Success());

            var handler = new LoginUserHandler(mockIdentityService.Object);
            var dummyData = new LoginUser { Email = "test@test.com", Password = "ValidPassword" };

            //Act
            var result = await handler.Handle(dummyData, default(CancellationToken));

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("AuthenticationTests", "LoginUser")]
        public async Task LoginUser_InValidCredentials_ShouldReturnFalse()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Fail());

            var handler = new LoginUserHandler(mockIdentityService.Object);
            var dummyData = new LoginUser { Email = "test1@test.com", Password = "InValidPassword" };

            //Act
            var result = await handler.Handle(dummyData, default(CancellationToken));

            //Assert
            Assert.False(result.IsSuccess);
        }
        #endregion
    }
}
