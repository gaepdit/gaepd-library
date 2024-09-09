namespace AppLibrary.Tests.EfRepositoryTests;

public class RepositoryTestBase
{
    protected EfRepositoryTestHelper Helper = default!;

    protected TestRepository Repository = default!;

    [SetUp]
    public void SetUp()
    {
        Helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        Repository = Helper.GetRepository();
    }

    [TearDown]
    public async Task TearDown()
    {
        await Repository.DisposeAsync();
        await Helper.DisposeAsync();
    }
}
