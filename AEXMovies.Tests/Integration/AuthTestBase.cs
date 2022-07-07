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
        var userManager = Application.Services.GetRequiredService<UserManager<User>>();
        await userManager.CreateAsync(user, password);
        return user;
    }

    public Task<User> CreateDefaultUser()
    {
        return CreateUser(DefaultUsername, DefaultPassword);
    }

    public async Task AuthorizeAs(User user, string password)
    {
        var response = await Client.PostAsync("/api/v1/auth/login", JsonContent.Create(new { login = user.UserName, password }));
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateAndAuthorizeDefaultUser()
    {
        var user = await CreateDefaultUser();
        await AuthorizeAs(user, DefaultPassword);
    }


    public AuthTestBase(TestingApplication testingApplication) : base(testingApplication)
    {
    }
}