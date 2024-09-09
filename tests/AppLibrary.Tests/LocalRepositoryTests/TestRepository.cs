using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

namespace AppLibrary.Tests.LocalRepositoryTests;

public class TestRepository(IEnumerable<TestEntity> items) : BaseRepository<TestEntity, Guid>(items)
{
    public static TestRepository GetRepository() =>
        new(new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Note = "Abc" },
            new() { Id = Guid.NewGuid(), Note = "Def" },
        });
}
