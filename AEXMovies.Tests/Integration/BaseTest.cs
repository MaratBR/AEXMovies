namespace AEXMovies.Tests.Integration;

public class BaseTest
{
    protected readonly TestingApplication Application;
    private HttpClient? _client;

    protected HttpClient Client => _client ??= Application.CreateClient();
    
    public BaseTest()
    {
        Application = new TestingApplication();
    }
}