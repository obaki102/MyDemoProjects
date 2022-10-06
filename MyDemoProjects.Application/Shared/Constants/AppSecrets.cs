using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Constants
{
    public static class AppSecrets
    {
        public const string GoogleClientId = "google_client_id";
        public const string GoogleClientSecret = "google_client_secret";
        public const string AnimelistClientId = "client_id";
        public const string TokenKey = "token_key";
        public const string DefaultConnectionString = "DefaultConnection";
        public const string Bearer = "Bearer";


        public static class LocalStorage
        {
            public const string  AuthToken = "auth_Token";
        }
    }
}
