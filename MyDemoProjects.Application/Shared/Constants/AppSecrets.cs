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
        public const string GoogleClientId = "googleClientId";
        public const string GoogleClientSecret = "googleClientSecret";
        public const string TokenKey = "tokenKey";
        public const string DefaultConnectionString = "DefaultConnection";
        public const string Bearer = "Bearer";


        public static class LocalStorage
        {
            public const string  AuthToken = "auth_Token";
        }

        public static class AnimeList
        {
            public const string XmalClientId = "X-MAL-CLIENT-ID";
            public const string AnimelistClientId = "clientId";
        }
    }
}
