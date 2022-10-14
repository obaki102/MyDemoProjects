using System.Text.Json.Serialization;

namespace MyDemoProjects.DiscordBot.DTOs
{
   
        public record Category(
        [property: JsonPropertyName("categoryName")] string CategoryName,
        [property: JsonPropertyName("entries")] Entries Entries,
        [property: JsonPropertyName("categoryId")] string CategoryId
    );

        public record Entries(
            [property: JsonPropertyName("results")] IReadOnlyList<Result> Results,
            [property: JsonPropertyName("totalResults")] int TotalResults,
            [property: JsonPropertyName("hasMore")] bool HasMore,
            [property: JsonPropertyName("query")] Query Query,
            [property: JsonPropertyName("useTotalResults")] bool UseTotalResults
        );

        public record MessageData(

        );

        public record Query(
            [property: JsonPropertyName("itemsPerPage")] int ItemsPerPage,
            [property: JsonPropertyName("currentPage")] int CurrentPage,
            [property: JsonPropertyName("tags")] IReadOnlyList<string> Tags,
            [property: JsonPropertyName("contentType")] string ContentType
        );

        public record Response(
            [property: JsonPropertyName("categories")] IReadOnlyList<Category> Categories
        );

        public record Result(
            [property: JsonPropertyName("weight")] double Weight,
            [property: JsonPropertyName("isFeatured")] bool IsFeatured,
            [property: JsonPropertyName("identifier")] string Identifier,
            [property: JsonPropertyName("entityType")] int EntityType,
            [property: JsonPropertyName("displayName")] string DisplayName,
            [property: JsonPropertyName("tagline")] string Tagline,
            [property: JsonPropertyName("image")] string Image,
            [property: JsonPropertyName("link")] string Link,
            [property: JsonPropertyName("webmVideo")] string WebmVideo,
            [property: JsonPropertyName("mp4Video")] string Mp4Video,
            [property: JsonPropertyName("featureImage")] string FeatureImage,
            [property: JsonPropertyName("creationDate")] DateTime CreationDate
        );

        public record GetTrendingCategoriesResponse(
            [property: JsonPropertyName("Response")] Response Response,
            [property: JsonPropertyName("ErrorCode")] int ErrorCode,
            [property: JsonPropertyName("ThrottleSeconds")] int ThrottleSeconds,
            [property: JsonPropertyName("ErrorStatus")] string ErrorStatus,
            [property: JsonPropertyName("Message")] string Message,
            [property: JsonPropertyName("MessageData")] MessageData MessageData
        );
    
}
