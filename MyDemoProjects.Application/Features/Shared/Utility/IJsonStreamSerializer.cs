namespace MyDemoProjects.Application.Features.Shared.Utility
{
    /// <summary>
    /// Contract for JsonStreamSerializer
    /// </summary>
    public interface IJsonStreamSerializer
    {
        /// <summary>
        /// Desirealize stream content to type of T.
        /// </summary>
        /// <typeparam name="T">Target type where  the stream is to be deserialized.</typeparam>
        /// <param name="streamContent">Stream input</param>
        T DeserializeStream<T> (Stream streamContent);
    }
}
