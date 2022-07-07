namespace AEXMovies.Tests.Integration;

public class AuthTest : AuthTestBase
{
    [Fact]
    public async Task CanAuthorize()
    {
        await CreateAndAuthorizeDefaultUser();
        var response = await Client.GetAsync("/api/v1/Auth/whoami");
        response.EnsureSuccessStatusCode();
    }

    public AuthTest(TestingApplication testingApplication) : base(testingApplication)
    {
    }
}