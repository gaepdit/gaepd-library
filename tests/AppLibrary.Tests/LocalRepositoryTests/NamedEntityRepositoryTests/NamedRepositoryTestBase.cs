namespace AppLibrary.Tests.LocalRepositoryTests.NamedEntityRepositoryTests;

public class NamedRepositoryTestBase
{
    protected TestNamedEntityRepository NamedEntityRepository;

    [SetUp]
    public void SetUp() => NamedEntityRepository = TestNamedEntityRepository.GetNamedEntityRepository();

    [TearDown]
    public async Task TearDown() => await NamedEntityRepository.DisposeAsync();
}
