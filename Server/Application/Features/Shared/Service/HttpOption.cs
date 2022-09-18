﻿   namespace MyDemoProjects.Server.Application.Features.Shared.Service
    {
        public class HttpServiceOption
        {
            /// <summary>
            /// True if token is needed
            /// </summary>
            public bool IsTokenRequired { get; set; } = false;

            /// <summary>
            /// Supply token value
            /// </summary>
            public string Token { get; set; } = string.Empty;

            /// <summary>
            /// Determine if refreshtoken is requested
            /// </summary>
            public bool IsRefreshToken { get; set; } = false;

            /// <summary>
            /// Specify endpoint to call
            /// </summary>
            public Uri Endpoint { get; set; }

            /// <summary>
            /// Specify Code
            /// </summary>
            public string Code { get; set; }


        }
    }


