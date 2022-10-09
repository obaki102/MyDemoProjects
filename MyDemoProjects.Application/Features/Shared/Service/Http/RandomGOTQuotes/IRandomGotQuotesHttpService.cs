using MyDemoProjects.Application.Features.ExternalApi.RandomGOTQuotes.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Features.Shared.Service.Http.RandomGOTQuotes
{
    public interface IRandomGotQuotesHttpService
    {
        Task<ApplicationResponse<RandomGOTQuotesResponse>> GetRandomQuotes();
    }
}
