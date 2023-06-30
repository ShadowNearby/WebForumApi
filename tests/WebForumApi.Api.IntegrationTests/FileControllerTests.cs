using WebForumApi.Api.IntegrationTests.Common;

namespace WebForumApi.Api.IntegrationTests;

public class FileControllerTests : BaseTest
{
    public FileControllerTests(CustomWebApplicationFactory apiFactory)
        : base(apiFactory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        LoginAsAdmin();
    }
}