using GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

namespace GaEpd.AppLibrary.Tests.RepositoryHelpers;

public class LocalRepository : BaseRepository<TestEntity, Guid>
{
    public LocalRepository(IEnumerable<TestEntity> items) : base(items) { }
}

public static class LocalRepositoryTestHelper
{
    public static LocalRepository GetTestRepository() =>
        new(new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Abc" },
            new() { Id = Guid.NewGuid(), Name = "Def" },
        });
}
