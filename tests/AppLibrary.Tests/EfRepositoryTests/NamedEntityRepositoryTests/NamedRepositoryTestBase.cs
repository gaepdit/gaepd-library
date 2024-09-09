namespace AppLibrary.Tests.EfRepositoryTests.NamedEntityRepositoryTests;

public class NamedRepositoryTestBase
{
    protected EfRepositoryTestHelper Helper = default!;

    protected TestNamedEntityRepository Repository = default!;

    [SetUp]
    public void SetUp()
    {
        Helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        Repository = Helper.GetNamedEntityRepository();
    }

    [TearDown]
    public async Task TearDown()
    {
        await Repository.DisposeAsync();
        await Helper.DisposeAsync();
    }
}
