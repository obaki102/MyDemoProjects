using System.Net;
using System.Text.Json;

namespace MyDemoProjects.Server.Shared.Utility
{
    public class JsonToServerResponse<T> : IJsonToServerResponse<T> where T : class
    {
        public async Task<ApplicationResponse<T>> Convert(HttpResponseMessage response)
        {
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<T>(content);
                    return new ApplicationResponse<T>
                    {
                        Data = result,
                        Message = response.StatusCode.ToString(),
                        Status = true
                    };
                }

                return new ApplicationResponse<T>
                {
                    Message = response.StatusCode.ToString(),
                    Status = false
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<T>
                {
                    Message = ex.Message,
                    Status = false
                };
            }
        }
    }
}
