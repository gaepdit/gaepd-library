using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests.IncludePropertiesTests;

public class NavigationPropertiesTestBase
{
    protected NavigationPropertiesRepository Repository;

    protected readonly List<TestEntityWithNavigationProperties> NavigationPropertyEntities =
    [
        new()
        {
            Id = Guid.NewGuid(),
            TextRecords =
            {
                new TextRecord { Id = Guid.NewGuid(), Text = "Abc" },
                new TextRecord { Id = Guid.NewGuid(), Text = "Def" },
            },
        },
    ];

    [SetUp]
    public void SetUp() => Repository = EfRepositoryTestHelper.CreateRepositoryHelper()
        .GetNavigationPropertiesRepository(NavigationPropertyEntities);

    [TearDown]
    public async Task TearDown() => await Repository.DisposeAsync();
}
