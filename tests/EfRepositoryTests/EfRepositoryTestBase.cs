using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class EfRepositoryTestBase
{
    protected EfRepositoryTestHelper Helper = default!;

    protected EfRepository Repository = default!;

    [SetUp]
    public void SetUp()
    {
        Helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        Repository = Helper.GetEfRepository();
    }

    [TearDown]
    public void TearDown() => Repository.Dispose();
}
