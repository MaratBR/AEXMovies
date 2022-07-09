namespace AEXMovies.Tests.Integration;

public class BaseTest
{
    protected readonly TestingApplication Application;
    private HttpClient? _client;

    public BaseTest()
    {
        Application = new TestingApplication();
    }

    protected HttpClient Client => _client ??= Application.CreateClient();
}