using AppLibrary.Tests.RepositoryHelpers;

namespace AppLibrary.Tests.LocalRepositoryTests;

public class LocalRepositoryTestBase
{
    protected DerivedLocalRepository Repository = default!;

    [SetUp]
    public void SetUp() => Repository = LocalRepositoryTestHelper.GetRepository();

    [TearDown]
    public async Task TearDown() => await Repository.DisposeAsync();
}
