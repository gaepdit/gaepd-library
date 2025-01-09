using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests.IncludePropertiesTests;

public class NavigationPropertiesTestBase
{
    protected NavigationPropertiesRepository Repository;

    protected static readonly List<TestEntityWithNavigationProperties> NavigationPropertyEntities =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Llama",
            TextRecords =
            {
                new TextRecord { Id = Guid.NewGuid(), Text = "A" },
                new TextRecord { Id = Guid.NewGuid(), Text = "B" },
            },
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Moose",
            TextRecords =
            {
                new TextRecord { Id = Guid.NewGuid(), Text = "A" },
                new TextRecord { Id = Guid.NewGuid(), Text = "B" },
            },
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Narwhal",
            TextRecords =
            {
                new TextRecord { Id = Guid.NewGuid(), Text = "A" },
                new TextRecord { Id = Guid.NewGuid(), Text = "B" },
            },
        },
    ];

    [SetUp]
    public void SetUp() => Repository = EfRepositoryTestHelper.CreateRepositoryHelper()
        .GetNavigationPropertiesRepository(NavigationPropertyEntities);

    [TearDown]
    public async Task TearDown() => await Repository.DisposeAsync();
}
