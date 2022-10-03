using System.Net;
using System.Text.Json;

namespace MyDemoProjects.Application.Features.Shared.Utility
{
#nullable disable
    /// <summary>
    /// A utility class for deserialization of stream input. 
    /// </summary>
    public class JsonStreamSerializer : IJsonStreamSerializer
    {
        /// <summary>
        /// Desirealize stream content to type of T.
        /// </summary>
        /// <typeparam name="T">Target type where  the stream is to be deserialized.</typeparam>
        /// <param name="streamContent">Stream input</param>
        /// <returns>A deserialized stream of type T</returns>
        public T DeserializeStream<T>(Stream streamContent)
        {
            return JsonSerializer.Deserialize<T>(streamContent);
        }
    }
}
