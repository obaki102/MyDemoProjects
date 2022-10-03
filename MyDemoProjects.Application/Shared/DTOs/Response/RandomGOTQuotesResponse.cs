using System.Text.Json.Serialization;
namespace MyDemoProjects.Application.Shared.DTOs.Response
{
    public record Character(
       [property: JsonPropertyName("name")] string Name,
       [property: JsonPropertyName("slug")] string Slug,
       [property: JsonPropertyName("house")] House House
   );

    public record House(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("slug")] string Slug
    );

    public record RandomGOTQuotesResponse(
        [property: JsonPropertyName("sentence")] string Sentence,
        [property: JsonPropertyName("character")] Character Character
    );
}
