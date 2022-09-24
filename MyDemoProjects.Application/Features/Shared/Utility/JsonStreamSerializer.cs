using System.Net;
using System.Text.Json;

namespace MyDemoProjects.Application.Features.Shared.Utility
{
    public class JsonStreamSerializer : IJsonStreamSerializer
    {
        public T DeserializeStream<T>(Stream streamContent)
        {
            return JsonSerializer.Deserialize<T>(streamContent);
        }
    }
}
