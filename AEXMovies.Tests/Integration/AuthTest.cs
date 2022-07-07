using System.Net;
using System.Net.Http.Json;

namespace AEXMovies.Tests.Integration;

public class AuthTest : AuthTestBase
{
    [Fact]
    public async Task CanAuthorize()
    {
        await CreateAndAuthorizeDefaultUser();
        var response = await Client.GetAsync("/api/v1/auth/whoami");
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task RefreshToken()
    {
        var result = await CreateAndAuthorizeDefaultUser();
        var response = await Client.PostAsync("/api/v1/auth/refresh", JsonContent.Create(new { refreshToken = result.RefreshToken }));
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Register()
    {
        var response = await Client.PostAsync("/api/v1/auth/register", JsonContent.Create(new { userName = "Test", password = "Password!@#3423" }));
        response.EnsureSuccessStatusCode();
        
        response = await Client.PostAsync("/api/v1/auth/register", JsonContent.Create(new { userName = "Test", password = "Password!@#3423" }));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}