using System.Net;
using System.Text.Json;

namespace MyDemoProjects.Server.Shared.Utility
{
    public class JsonToServerResponse<T> : IJsonToServerResponse<T> where T : class
    {
        public async Task<ServerResponse<T>> Convert(HttpResponseMessage response)
        {
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<T>(content);
                    return new ServerResponse<T>
                    {
                        Data = result,
                        Message = response.StatusCode.ToString(),
                        Status = true
                    };
                }

                return new ServerResponse<T>
                {
                    Message = response.StatusCode.ToString(),
                    Status = false
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<T>
                {
                    Message = ex.Message,
                    Status = false
                };
            }
        }
    }
}
