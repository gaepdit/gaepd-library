using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

namespace AppLibrary.Tests.LocalRepositoryTests.NamedEntityRepositoryTests;

public class TestNamedEntityRepository(IEnumerable<TestNamedEntity> items)
    : NamedEntityRepository<TestNamedEntity>(items)
{
    public const string UsefulSuffix = "def";

    public static TestNamedEntityRepository GetNamedEntityRepository() =>
        new(new List<TestNamedEntity>
        {
            new(id: Guid.NewGuid(), name: "Abc abc"),
            new(id: Guid.NewGuid(), name: $"Xyx {UsefulSuffix}"),
            new(id: Guid.NewGuid(), name: $"Efg {UsefulSuffix}"),
        });
}
