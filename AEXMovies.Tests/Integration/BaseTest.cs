namespace AEXMovies.Tests.Integration;

public class BaseTest : IDisposable, IClassFixture<TestingApplication>
{
    protected readonly TestingApplication Application;
    private HttpClient? _client;

    protected HttpClient Client => _client ??= Application.CreateClient();
    
    public BaseTest(TestingApplication testingApplication)
    {
        Application = testingApplication;
    }

    public void Dispose()
    {
        Application.Dispose();
    }
}