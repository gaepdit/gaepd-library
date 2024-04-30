namespace AppLibrary.Tests.EfRepositoryTests;

public class EfRepositoryTestBase
{
    protected EfRepositoryTestHelper Helper = default!;

    protected DerivedEfRepository Repository = default!;

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
