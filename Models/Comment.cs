using Newtonsoft.Json;

namespace RestTestsKozlenko.Models;

public class Comment
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("postId")]
    public int PostId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("body")]
    public string Body { get; set; } = string.Empty;
}
