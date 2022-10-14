using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyDemoProjects.DiscordBot.DTOs
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

    public record Embed(
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("color")] int Color,
        [property: JsonPropertyName("footer")] Footer Footer
    );

    public record Footer(
        [property: JsonPropertyName("text")] string Text
    );

    public record Root(
        string Content,
        [property: JsonPropertyName("embeds")] List<Embed> Embeds
    );

}
