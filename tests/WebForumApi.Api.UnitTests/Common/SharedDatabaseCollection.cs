using WebForumApi.Api.UnitTests.Common;
using Xunit;

namespace WebForumApi.Api.UnitTests.Common;

[CollectionDefinition("Test collection")]
public class SharedDatabaseCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}