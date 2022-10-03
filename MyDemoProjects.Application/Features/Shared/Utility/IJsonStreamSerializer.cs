namespace MyDemoProjects.Application.Features.Shared.Utility
{
    /// <summary>
    /// Contract for JsonStreamSerializer
    /// </summary>
    public interface IJsonStreamSerializer
    {
       T DeserializeStream<T> (Stream streamContent);
    }
}
