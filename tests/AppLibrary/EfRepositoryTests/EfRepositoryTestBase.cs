using AppLibrary.Tests.RepositoryHelpers;

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
    public void TearDown()
    {
        Repository.Dispose();
        Helper.Dispose();
    }
}
