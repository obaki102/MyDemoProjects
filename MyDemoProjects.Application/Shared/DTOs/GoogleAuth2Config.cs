using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.DTOs
{
    public  class GoogleAuth2Config
    {
        public string ClientId { get; init; } = string.Empty;
        public string Scope { get; init; } = string.Empty;
        public string AccessToken { get; init; } = string.Empty;
        public string DiscoveryDocs { get; init; } = string.Empty;
    }
}
