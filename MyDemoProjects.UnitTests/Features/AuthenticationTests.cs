using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Features.Authentication.Queries;
using MyDemoProjects.Application.Infastructure.Identity.Models;
using MyDemoProjects.Application.Infastructure.Identity.Services;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;
using System.Security.Claims;

namespace MyDemoProjects.UnitTests.Features
{
    public class AuthenticationTests
    {

        #region LoginUser
        [Fact]
        [Trait("Commands", "LoginUser")]
        public async Task LoginUser_ValidCredentials_ShouldReturnTrue()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginUserAsync(It.Is<string>(s => s == "test@test.com"), It.Is<string>(p => p == "ValidPassword")))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Success());

            var handler = new LoginUserHandler(mockIdentityService.Object);
            var dummyData = new LoginUser { EmailAddress = "test@test.com", Password = "ValidPassword" };

            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("Commands", "LoginUser")]
        public async Task LoginUser_InValidCredentials_ShouldReturnFalse()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Fail());

            var handler = new LoginUserHandler(mockIdentityService.Object);
            var dummyData = new LoginUser { EmailAddress = "test1@test.com", Password = "InValidPassword" };

            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        [Trait("Commands", "LoginUser")]
        public async Task LoginUser_ValidCredentials_ShouldReturnToken()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginUserAsync(It.Is<string>(s => s == "test@test.com"), It.Is<string>(p => p == "ValidPassword")))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Success(new TokenResponse(HelperMethods.GenerateDummyToken())));

            var handler = new LoginUserHandler(mockIdentityService.Object);
            var dummyData = new LoginUser { EmailAddress = "test@test.com", Password = "ValidPassword" };

            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.NotNull(result.Data);
        }
        #endregion

        #region LoginExternalUserHandler
        [Fact]
        [Trait("Commands", "LoginExternalUser")]
        public async Task LoginExternalUser_InValidCredentials_ShouldReturnFalse()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginExternalUserAsync(It.IsAny<LoginExternalUser>()))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Fail());

            var handler = new LoginExternalUserHandler(mockIdentityService.Object);
            var dummyData = new LoginExternalUser { EmailAddress = "test1@test.com",  Provider ="Google"};

            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        [Trait("Commands", "LoginExternalUser")]
        public async Task LoginExternalUser_ValidCredentials_ShouldReturnTrue()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginExternalUserAsync(It.IsAny<LoginExternalUser>()))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Success());

            var handler = new LoginExternalUserHandler(mockIdentityService.Object);
            var dummyData = new LoginExternalUser { EmailAddress = "test@test.com", Provider = "Google" };

            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("Commands", "LoginExternalUser")]
        public async Task LoginExternalUser_ValidCredentials_ShouldReturnToken()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.LoginExternalUserAsync(It.IsAny<LoginExternalUser>()))
                        .ReturnsAsync(ApplicationResponse<TokenResponse>.Success(new TokenResponse(HelperMethods.GenerateDummyToken())));

            var handler = new LoginExternalUserHandler(mockIdentityService.Object);
            var dummyData = new LoginExternalUser { EmailAddress = "test@test.com", Provider = "Google" };

            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.NotNull(result.Data);
        }
        #endregion LoginExternalUserHandler

        #region ValidateToken
        [Fact]
        [Trait("Commands", "ValidateToken")]
        public  void ValidateToken_ValidToken_ShouldReturnTrue()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.ValidateTokenAndGetClaimsPrincipal(It.IsAny<string>()))
                        .Returns(ApplicationResponse<ClaimsPrincipal>.Success());

            var handler = new ValidateTokenHandler(mockIdentityService.Object);
            var dummyData = new ValidateToken(HelperMethods.GenerateDummyToken());

            //Act
            var result =  handler.Handle(dummyData, default);

            //Assert
            Assert.True(result.Result.IsSuccess);
        }

        [Fact]
        [Trait("Commands", "ValidateToken")]
        public void ValidateToken_InValidToken_ShouldReturnFalse()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.ValidateTokenAndGetClaimsPrincipal(It.IsAny<string>()))
                        .Returns(ApplicationResponse<ClaimsPrincipal>.Fail());

            var handler = new ValidateTokenHandler(mockIdentityService.Object);
            var dummyData = new ValidateToken("InvalidToken");

            //Act
            var result = handler.Handle(dummyData, default);

            //Assert
            Assert.False(result.Result.IsSuccess);
        }
        #endregion

        #region ChangePassword
        [Fact]
        [Trait("Commands", "ChangePassword")]
        public async Task ChangePassword_ValidPassword_ShouldReturnTrue()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(ApplicationResponse<bool>.Success());

            var handler = new ChangePasswordHandler(mockIdentityService.Object);
            var dummyData = new ChangePassword
            {
                EmailAddress = "test@test.com",
                CurrentPassword = "currentPassword",
                NewPassword = "newPassword"
            };
            //Act
            var result =await handler.Handle(dummyData, default);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("Commands", "ChangePassword")]
        public async Task ChangePassword_InValidPassword_ShouldReturnFalse()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(ApplicationResponse<bool>.Fail());

            var handler = new ChangePasswordHandler(mockIdentityService.Object);
            var dummyData = new ChangePassword
            {
                EmailAddress = "test@test.com",
                CurrentPassword = "currentPassword",
                NewPassword = "newPassword"
            };
            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.False(result.IsSuccess);
        }
        #endregion

        #region CreateAccount

        [Fact]
        [Trait("Commands", "CreateAccount")]
        public async Task CreateAccount_ValidAccount_ShouldReturnTrue()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(ApplicationResponse<bool>.Success());

            var mockImapper = new Mock<IMapper>();
            mockImapper.Setup(x => x.Map(It.IsAny<CreateAccount>(), It.IsAny<ApplicationUser>()))
                        .Returns(new ApplicationUser { Email = "test@test.com" });
                        

            var handler = new CreateAccountHandler(mockIdentityService.Object, mockImapper.Object);
            var dummyData = new CreateAccount
            {
                Email = "test@test.com"
            };
            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("Commands", "CreateAccount")]
        public async Task CreateAccount_InValidAccount_ShouldReturnFalse()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(ApplicationResponse<bool>.Fail());

            var mockImapper = new Mock<IMapper>();
            mockImapper.Setup(x => x.Map(It.IsAny<CreateAccount>(), It.IsAny<ApplicationUser>()))
                        .Returns(new ApplicationUser { Email = "test@test.com" });


            var handler = new CreateAccountHandler(mockIdentityService.Object, mockImapper.Object);
            var dummyData = new CreateAccount
            {
                Email = "InvalidAccount"
            };
            //Act
            var result = await handler.Handle(dummyData, default);

            //Assert
            Assert.False(result.IsSuccess);
        }
        #endregion

        #region GetAllUsers
        [Fact]
        [Trait("Queries", "GetAllUsers")]
        public async Task CGetAllUsers_UsersFound_ShouldReturnTrue()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.GetAllUsersAsync())
                        .ReturnsAsync(ApplicationResponse<IEnumerable<ApplicationUser>>.Success());

            var mockImapper = new Mock<IMapper>();
            mockImapper.Setup(x => x.Map(It.IsAny<IEnumerable<ApplicationUser>>(), It.IsAny<IEnumerable<UserDetailsResponse>>()))
                        .Returns(new List<UserDetailsResponse>
                        { 
                            new UserDetailsResponse{ },
                            new UserDetailsResponse { }
                        });

            var handler = new GetAllUsersHandler(mockIdentityService.Object, mockImapper.Object);
            //Act
            var result = await handler.Handle(new GetAllUsers(),default);

            //Assert
            Assert.NotNull(result.Data);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("Queries", "GetAllUsers")]
        public async Task CGetAllUsers_NoUsersFound_ShouldReturnFalse()
        {
            //Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup(x => x.GetAllUsersAsync())
                        .ReturnsAsync(ApplicationResponse<IEnumerable<ApplicationUser>>.Fail());

            var mockImapper = new Mock<IMapper>();
            mockImapper.Setup(x => x.Map(It.IsAny<IEnumerable<ApplicationUser>>(), It.IsAny<IEnumerable<UserDetailsResponse>>()))
                        .Returns(new List<UserDetailsResponse>
                        {
                            new UserDetailsResponse{ },
                            new UserDetailsResponse { }
                        });

            var handler = new GetAllUsersHandler(mockIdentityService.Object, mockImapper.Object);
            //Act
            var result = await handler.Handle(new GetAllUsers(), default);

            //Assert
            Assert.Null(result.Data);
            Assert.False(result.IsSuccess);
        }
        #endregion region
    }
}
