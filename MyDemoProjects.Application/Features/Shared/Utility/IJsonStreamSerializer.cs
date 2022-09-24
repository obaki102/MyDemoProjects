namespace MyDemoProjects.Application.Features.Shared.Utility
{
    public interface IJsonStreamSerializer
    {
       T DeserializeStream<T> (Stream streamContent);
    }
}
