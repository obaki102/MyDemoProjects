using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using MyDemoProjects.Application.Features.Shared.Service.Http.AnimeList;
using MyDemoProjects.Application.Features.Shared.Utility;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;
using System.Net;
using Xunit.Abstractions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace MyDemoProjects.UnitTests.Services.ExternalApi
{
    public class AnimeListHttpServiceTests
    {
        public  IAnimeListHttpService? _animeListHttpService;
        public  IConfiguration _configuration;
        public  ITestOutputHelper _output;

        public AnimeListHttpServiceTests(ITestOutputHelper output)
        {
            _output = output;
            //Mock configuration
            var mockConfiguration = new Mock<IConfiguration>();
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(a => a.Value).Returns("someclientId");
            mockConfiguration.Setup(a => a.GetSection(AppSecrets.AnimeList.AnimelistClientId)).Returns(mockConfigurationSection.Object);
            _configuration = mockConfiguration.Object;
        }

        [Fact]
        [Trait("AnimeListHttpServiceTests", "GetAnimeListBySeasonAndYear")]
        public async Task GetAnimeListBySeasonAndYear_Http200_ShouldReturnTrue()
        {
            //Arrange
            var mockJsonSerializer = new Mock<IJsonStreamSerializer>();
            mockJsonSerializer.Setup(x => x.DeserializeStream<ApplicationResponse<AnimeListRoot>>(It.IsAny<Stream>()))
                .Returns(ApplicationResponse<AnimeListRoot>.Success());

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect("https://api.myanimelist.net/*")
                    .Respond(HttpStatusCode.OK);

            var httpClient = mockHttp.ToHttpClient();
            _animeListHttpService = new AnimeListHttpService(httpClient, mockJsonSerializer.Object, _configuration);
            var season = new Season(2022,"fall");

            //Act
            var response = await _animeListHttpService.GetAnimeListBySeasonAndYear(season);
            _output.WriteLine(response.Messages[0]);

            //Assert
            Assert.True(response.IsSuccess);
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        [Trait("AnimeListHttpServiceTests", "GetAnimeListBySeasonAndYear")]
        public async Task GetAnimeListBySeasonAndYear_Http401_ShouldReturnFalse()
        {
            //Arrange
            var mockJsonSerializer = new Mock<IJsonStreamSerializer>();
            mockJsonSerializer.Setup(x => x.DeserializeStream<ApplicationResponse<AnimeListRoot>>(It.IsAny<Stream>()))
                .Returns(ApplicationResponse<AnimeListRoot>.Success());

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect("https://api.myanimelist.net/*")
                    .Respond(HttpStatusCode.Unauthorized);

            var httpClient = mockHttp.ToHttpClient();
            _animeListHttpService = new AnimeListHttpService(httpClient, mockJsonSerializer.Object, _configuration);
            var season = new Season(2022, "fall");

            //Act
            var response = await _animeListHttpService.GetAnimeListBySeasonAndYear(season);
            _output.WriteLine(response.Messages[0]);
            //Assert
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Messages[0], "Unauthorized");
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        [Trait("AnimeListHttpServiceTests", "GetAnimeListBySeasonAndYear")]
        public async Task GetAnimeListBySeasonAndYear_Http403__ShouldReturnFalse()
        {
            //Arrange
            var mockJsonSerializer = new Mock<IJsonStreamSerializer>();
            mockJsonSerializer.Setup(x => x.DeserializeStream<ApplicationResponse<AnimeListRoot>>(It.IsAny<Stream>()))
                .Returns(ApplicationResponse<AnimeListRoot>.Success());

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect("https://api.myanimelist.net/*")
                    .Respond(HttpStatusCode.Forbidden);

            var httpClient = mockHttp.ToHttpClient();
            _animeListHttpService = new AnimeListHttpService(httpClient, mockJsonSerializer.Object, _configuration);
            var season = new Season(2022, "fall");

            //Act
            var response = await _animeListHttpService.GetAnimeListBySeasonAndYear(season);
            _output.WriteLine(response.Messages[0]);
            //Assert
            Assert.False(response.IsSuccess);
            mockHttp.VerifyNoOutstandingExpectation();
        }



    }
}

