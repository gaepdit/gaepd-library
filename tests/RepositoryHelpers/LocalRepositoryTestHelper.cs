using GaEpd.AppLibrary.Domain.Repositories.LocalRepository;
using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.RepositoryHelpers;

public class DerivedLocalRepository : BaseRepository<DerivedEntity, Guid>
{
    public DerivedLocalRepository(IEnumerable<DerivedEntity> items) : base(items) { }
}

public class DerivedLocalNamedEntityRepository : NamedEntityRepository<DerivedNamedEntity>
{
    public DerivedLocalNamedEntityRepository(IEnumerable<DerivedNamedEntity> items) : base(items) { }
}

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
