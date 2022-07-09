using System.Net.Http.Headers;
using System.Net.Http.Json;
using AEXMovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AEXMovies.Tests.Integration;

public class AuthTestBase : BaseTest
{
    protected const string DefaultUsername = "DefaultUser";
    protected const string DefaultPassword = "Pa$$word123$%$#";


    public async Task<User> CreateUser(string userName, string password)
    {
        var user = new User
        {
            UserName = userName
        };
        var scopeFactory = Application.Services.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await userManager.CreateAsync(user, password);
        }

        return user;
    }

    public Task<User> CreateDefaultUser()
    {
        return CreateUser(DefaultUsername, DefaultPassword);
    }

    public async Task<AuthorizationResult> AuthorizeAs(User user, string password)
    {
        var response = await Client.PostAsync("/api/v1/auth/login",
            JsonContent.Create(new { login = user.UserName, password }));
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(data);
        Assert.True(data.ContainsKey("token"));
        Assert.True(data.ContainsKey("refreshToken"));
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data["token"]);
        return new AuthorizationResult(user, password, data["token"], data["refreshToken"]);
    }

    public async Task<AuthorizationResult> CreateAndAuthorizeDefaultUser()
    {
        var user = await CreateDefaultUser();
        return await AuthorizeAs(user, DefaultPassword);
    }
}