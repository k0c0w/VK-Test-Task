using System.Text.Json.Serialization;

namespace WebAPI.Models;

public record UserGroupVM
{
    [JsonPropertyName("code")]
    [JsonPropertyOrder(0)]
    public string Code { get; init; }
    
    [JsonPropertyName("description")]
    [JsonPropertyOrder(1)]
    public string Description { get; init; }
}