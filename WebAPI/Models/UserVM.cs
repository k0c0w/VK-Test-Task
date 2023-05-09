
using System.Text.Json.Serialization;

namespace WebAPI.Models;

public record UserVM
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public int Id { get; init; }
    
    [JsonPropertyName("login")]
    [JsonPropertyOrder(1)]
    public string Login { get; init; }
    
    [JsonPropertyName("created_date")]
    [JsonPropertyOrder(3)]
    public DateOnly CreatedDate { get; init; }
    
    [JsonPropertyName("user_state")]
    [JsonPropertyOrder(4)]
    public UserStateVM State { get; init; }
    
    [JsonPropertyName("user_group")]
    [JsonPropertyOrder(5)]
    public UserGroupVM Group { get; init; }
}