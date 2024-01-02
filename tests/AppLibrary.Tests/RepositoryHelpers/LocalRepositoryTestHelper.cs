using AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

namespace AppLibrary.Tests.RepositoryHelpers;

public class DerivedLocalRepository(IEnumerable<DerivedEntity> items) : BaseRepository<DerivedEntity, Guid>(items);

public class DerivedLocalNamedEntityRepository(IEnumerable<DerivedNamedEntity> items)
    : NamedEntityRepository<DerivedNamedEntity>(items);

public static class LocalRepositoryTestHelper
{
    public static DerivedLocalRepository GetRepository() =>
        new(new List<DerivedEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Abc" },
            new() { Id = Guid.NewGuid(), Name = "Def" },
        });

    public static DerivedLocalNamedEntityRepository GetNamedEntityRepository() =>
        new(new List<DerivedNamedEntity>
        {
            new(Guid.NewGuid(), "Abc def"),
            new(Guid.NewGuid(), "Efg hij"),
        });
}
