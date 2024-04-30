using AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

namespace AppLibrary.Tests.LocalRepositoryTests;

public class DerivedLocalRepository(IEnumerable<DerivedEntity> items)
    : BaseRepository<DerivedEntity, Guid>(items)
{
    public static DerivedLocalRepository GetRepository() =>
        new(new List<DerivedEntity>
        {
            new() { Id = Guid.NewGuid(), Note = "Abc" },
            new() { Id = Guid.NewGuid(), Note = "Def" },
        });
}

public class LocalRepositoryTestBase
{
    protected DerivedLocalRepository Repository = default!;

    [SetUp]
    public void SetUp() => Repository = DerivedLocalRepository.GetRepository();

    [TearDown]
    public async Task TearDown() => await Repository.DisposeAsync();
}
