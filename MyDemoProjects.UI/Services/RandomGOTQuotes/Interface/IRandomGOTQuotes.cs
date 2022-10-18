using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.RandomGOTQuotes.Interface
{
    public interface IRandomGOTQuotes
    {
        RandomGOTQuotesResponse GOTQuotes { get; set; }
        Task<RandomGOTQuotesResponse> GetRandomGOTQuotes();
    }
}

