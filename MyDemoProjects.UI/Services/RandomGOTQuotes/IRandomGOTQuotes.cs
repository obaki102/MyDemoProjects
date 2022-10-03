using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.RandomGOTQuotes
{
    public interface IRandomGOTQuotes
    {
        RandomGOTQuotesResponse GOTQuotes { get; set; }
        Task<RandomGOTQuotesResponse> GetRandomGOTQuotes();
    }
}

