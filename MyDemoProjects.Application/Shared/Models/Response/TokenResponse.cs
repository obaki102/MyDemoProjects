using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Models.Response
{
    public record TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserImageURL { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
