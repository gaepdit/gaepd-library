using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class LocalRepositoryTestBase
{
    protected LocalRepository Repository = default!;

    [SetUp]
    public void SetUp() => Repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => Repository.Dispose();
}
