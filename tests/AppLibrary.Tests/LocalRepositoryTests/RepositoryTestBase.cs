namespace AppLibrary.Tests.LocalRepositoryTests;

public class RepositoryTestBase
{
    protected TestRepository Repository = default!;

    [SetUp]
    public void SetUp() => Repository = TestRepository.GetRepository();

    [TearDown]
    public async Task TearDown() => await Repository.DisposeAsync();
}
