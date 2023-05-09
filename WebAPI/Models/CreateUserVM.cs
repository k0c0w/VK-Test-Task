using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models;

public class CreateUserVM
{
    [Required]
    [MaxLength(128, ErrorMessage = "Max login length - 128")]
    [MinLength(1, ErrorMessage = "login can not be empty")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "for login use latin letters only")]
    [JsonPropertyName("login")]
    [JsonPropertyOrder(1)]
    public string Login { get; init; }
    
    [Required(ErrorMessage = "password is required")]
    [MinLength(6, ErrorMessage = "for password use at least 6 symbols")]
    [RegularExpression(@"^[a-zA-Z0-9\$_@#&!~]+$", ErrorMessage = "for password use digits, latin letters and $_@#&!~ only")]
    [JsonPropertyName("password")]
    [JsonPropertyOrder(2)]
    public string Password { get; init; }

    [Required(ErrorMessage = "user_group_id is required")]
    [Range(1,2, ErrorMessage = "user_group_id must be in range [1;2]")]
    [JsonPropertyName("user_group_id")]
    [JsonPropertyOrder(3)]
    public int GroupId { get; init; }
}