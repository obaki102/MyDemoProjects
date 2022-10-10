using Moq;
using MyDemoProjects.Application.Features.Shared.Service.Http.RandomGOTQuotes;
using MyDemoProjects.Application.Features.Shared.Utility;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;
using System.Net;

namespace MyDemoProjects.UnitTests.Services.ExternalApi
{
    public class RandomGotQuotesHttpServiceTests
    {
        public IRandomGotQuotesHttpService? _randomGotQuotesHttpService;


        [Fact]
        [Trait("RandomGotQuotesHttpService", "GetRandomQuotes")]
        public async Task GetRandomQuotes_Http200_ShouldReturnTrue()
        {
            //Arrange
            var mockJsonSerializer = new Mock<IJsonStreamSerializer>();
            mockJsonSerializer.Setup(x => x.DeserializeStream<ApplicationResponse<AnimeListRoot>>(It.IsAny<Stream>()))
                .Returns(ApplicationResponse<AnimeListRoot>.Success());

            var mockHttpHander = new MockHttpMessageHandler();
            mockHttpHander.When("https://api.gameofthronesquotes.xyz/*")
                          .Respond(HttpStatusCode.OK);

            var mockHttpClient = mockHttpHander.ToHttpClient();
            _randomGotQuotesHttpService = new RandomGotQuotesHttpService(mockHttpClient, mockJsonSerializer.Object);

            //Act
            var result = await _randomGotQuotesHttpService.GetRandomQuotes();

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("RandomGotQuotesHttpService", "GetRandomQuotes")]
        public async Task GetRandomQuotes_Http401_ShouldReturnFalse()
        {
            //Arrange
            var mockJsonSerializer = new Mock<IJsonStreamSerializer>();
            mockJsonSerializer.Setup(x => x.DeserializeStream<ApplicationResponse<AnimeListRoot>>(It.IsAny<Stream>()))
                .Returns(ApplicationResponse<AnimeListRoot>.Success());

            var mockHttpHander = new MockHttpMessageHandler();
            mockHttpHander.When("https://api.gameofthronesquotes.xyz/*")
                          .Respond(HttpStatusCode.Unauthorized);

            var mockHttpClient = mockHttpHander.ToHttpClient();
            _randomGotQuotesHttpService = new RandomGotQuotesHttpService(mockHttpClient, mockJsonSerializer.Object);

            //Act
            var result = await _randomGotQuotesHttpService.GetRandomQuotes();

            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        [Trait("RandomGotQuotesHttpService", "GetRandomQuotes")]
        public async Task GetRandomQuotes_Http206_ShouldReturnTrue()
        {
            //Arrange
            var mockJsonSerializer = new Mock<IJsonStreamSerializer>();
            mockJsonSerializer.Setup(x => x.DeserializeStream<ApplicationResponse<AnimeListRoot>>(It.IsAny<Stream>()))
                .Returns(ApplicationResponse<AnimeListRoot>.Success());

            var mockHttpHander = new MockHttpMessageHandler();
            mockHttpHander.When("https://api.gameofthronesquotes.xyz/*")
                          .Respond(HttpStatusCode.PartialContent);

            var mockHttpClient = mockHttpHander.ToHttpClient();
            _randomGotQuotesHttpService = new RandomGotQuotesHttpService(mockHttpClient, mockJsonSerializer.Object);

            //Act
            var result = await _randomGotQuotesHttpService.GetRandomQuotes();

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Trait("RandomGotQuotesHttpService", "GetRandomQuotes")]
        public async Task GetRandomQuotes_Http400_ShouldReturnFalse()
        {
            //Arrange
            var mockJsonSerializer = new Mock<IJsonStreamSerializer>();
            mockJsonSerializer.Setup(x => x.DeserializeStream<ApplicationResponse<AnimeListRoot>>(It.IsAny<Stream>()))
                .Returns(ApplicationResponse<AnimeListRoot>.Success());

            var mockHttpHander = new MockHttpMessageHandler();
            mockHttpHander.When("https://api.gameofthronesquotes.xyz/*")
                          .Respond(HttpStatusCode.BadRequest);

            var mockHttpClient = mockHttpHander.ToHttpClient();
            _randomGotQuotesHttpService = new RandomGotQuotesHttpService(mockHttpClient, mockJsonSerializer.Object);

            //Act
            var result = await _randomGotQuotesHttpService.GetRandomQuotes();

            //Assert
            Assert.False(result.IsSuccess);
        }
    }
}
