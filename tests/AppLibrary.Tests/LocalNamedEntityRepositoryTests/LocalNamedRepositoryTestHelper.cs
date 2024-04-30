using AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

namespace AppLibrary.Tests.LocalNamedEntityRepositoryTests;

public class DerivedLocalNamedEntityRepository(IEnumerable<DerivedNamedEntity> items)
    : NamedEntityRepository<DerivedNamedEntity>(items)
{
    public const string UsefulSuffix = "def";
    public static DerivedLocalNamedEntityRepository GetNamedEntityRepository() =>
        new(new List<DerivedNamedEntity>
        {
            new(id: Guid.NewGuid(), name: "Abc abc"),
            new(id: Guid.NewGuid(), name: $"Xyx {UsefulSuffix}"),
            new(id: Guid.NewGuid(), name: $"Efg {UsefulSuffix}"),
        });
}

public class LocalNamedRepositoryTestBase
{
    protected DerivedLocalNamedEntityRepository NamedRepository = default!;

    [SetUp]
    public void SetUp()
    {
        NamedRepository = DerivedLocalNamedEntityRepository.GetNamedEntityRepository();
    }

    [TearDown]
    public async Task TearDown()
    {
        await NamedRepository.DisposeAsync();
    }
}
