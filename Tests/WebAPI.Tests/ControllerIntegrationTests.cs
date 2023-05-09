using System.Net.Http.Json;

namespace WebAPI.Tests;

public class ControllerIntegrationTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;
    
    public ControllerIntegrationTests(TestApplicationFactory fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetUsers_IsDeniedForUnauthorized()
    {
        using var response = await _client.GetAsync("/users");
        
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateUser_AllowsAnonymousCreation()
    {
        using var response = await PostUserCreationAsync(new
        {
            login = "login",
            password = "password",
            user_group_id = 1
        });
        
        Assert.True(response.IsSuccessStatusCode);
    }
    
    [Fact]
    public async Task GetUsers_IsAccessedForAuthorized()
    {
        await PostUserCreationAsync(new { login = "superuser", password = "superpassword", user_group_id = 2 });
        var request = new HttpRequestMessage(HttpMethod.Get, "/users")
            { Headers = { {"Authorization", "Basic ^˩zZ("} } };
        
        using var response = await _client.SendAsync(request);
        
        Assert.True(response.IsSuccessStatusCode);
    }


    private async Task<HttpResponseMessage> PostUserCreationAsync(object user) => await _client.PostAsJsonAsync("/users/create", user);
}