using GaEpd.AppLibrary.Domain.Repositories.EFRepository;
using GaEpd.AppLibrary.Tests.EntityHelpers;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TestSupport.EfHelpers;

namespace GaEpd.AppLibrary.Tests.RepositoryHelpers;

public class EfRepository : BaseRepository<TestEntity, Guid>
{
    public EfRepository(DbContext context) : base(context) { }
}

public class EfNamedEntityRepository : NamedEntityRepository<TestNamedEntity>
{
    public EfNamedEntityRepository(DbContext context) : base(context) { }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TestEntity> TestEntities => Set<TestEntity>();
    public DbSet<TestNamedEntity> TestNamedEntities => Set<TestNamedEntity>();
}

public sealed class EfRepositoryTestHelper : IDisposable
{
    private AppDbContext Context { get; set; } = default!;
    private readonly DbContextOptions<AppDbContext> _options;
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor used by <see cref="CreateRepositoryHelper"/>.
    /// </summary>
    private EfRepositoryTestHelper()
    {
        _options = SqliteInMemory.CreateOptions<AppDbContext>();
        _context = new AppDbContext(_options);
        _context.Database.EnsureCreated();
    }

    /// <summary>
    /// Constructor used by <see cref="CreateSqlServerRepositoryHelper"/>.
    /// </summary>
    /// <param name="callingClass">The class of the unit test method requesting the Repository Helper.</param>
    /// <param name="callingMember">The unit test method requesting the Repository Helper.</param>
    private EfRepositoryTestHelper(object callingClass, string callingMember)
    {
        _options = callingClass.CreateUniqueMethodOptions<AppDbContext>(callingMember: callingMember,
            builder: opts => opts.UseSqlServer());
        _context = new AppDbContext(_options);
        _context.Database.EnsureClean();
    }

    /// <summary>
    /// Creates a new Sqlite database and returns a RepositoryHelper.
    /// </summary>
    /// <example>
    /// <para>
    /// Create an instance of a <c>RepositoryHelper</c> and a <c>Repository</c> like this:
    /// </para>
    /// <code>
    /// using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
    /// using var repository = repositoryHelper.GetOfficeRepository();
    /// </code>
    /// </example>
    /// <returns>An <see cref="EfRepositoryTestHelper"/> with an empty Sqlite database.</returns>
    public static EfRepositoryTestHelper CreateRepositoryHelper() => new();

    /// <summary>
    /// <para>
    /// Creates a SQL Server database and returns a RepositoryHelper. Use of this method requires that
    /// an "appsettings.json" exists in the project root with a connection string named "UnitTestConnection".
    /// </para>
    /// <para>
    /// (The "<c>callingClass</c>" and "<c>callingMember</c>" parameters are used to generate a unique
    /// database for each unit test method.)
    /// </para>
    /// </summary>
    /// <example>
    /// <para>
    /// Create an instance of a <c>RepositoryHelper</c> and a <c>Repository</c> like this:
    /// </para>
    /// <code>
    /// using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
    /// using var repository = repositoryHelper.GetOfficeRepository();
    /// </code>
    /// </example>
    /// <param name="callingClass">
    /// Enter "<c>this</c>". The class of the unit test method requesting the Repository Helper.
    /// </param>
    /// <param name="callingMember">
    /// Do not enter. The unit test method requesting the Repository Helper. This is filled in by the compiler.
    /// </param>
    /// <returns>A <see cref="EfRepositoryTestHelper"/> with a clean SQL Server database.</returns>
    public static EfRepositoryTestHelper CreateSqlServerRepositoryHelper(
        object callingClass, [CallerMemberName] string callingMember = "") =>
        new(callingClass, callingMember);

    /// <summary>
    /// Stops tracking all currently tracked entities.
    /// ReSharper disable once CommentTypo
    /// See https://github.com/JonPSmith/EfCore.TestSupport/wiki/Using-SQLite-in-memory-databases#1-best-approach-one-instance-and-use-changetrackerclear
    /// </summary>
    public void ClearChangeTracker() => Context.ChangeTracker.Clear();

    /// <summary>
    /// Deletes all data from the EF database table for the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity whose data is to be deleted.</typeparam>
    private async Task ClearTableAsync<TEntity>() where TEntity : class
    {
        Context.RemoveRange(Context.Set<TEntity>());
        await Context.SaveChangesAsync();
        ClearChangeTracker();
    }

    /// <summary>
    /// Deletes all data from the EF database table for the Test Entity.
    /// </summary>
    public async Task ClearTestEntityTableAsync() => await ClearTableAsync<TestEntity>();

    /// <summary>
    /// Seeds data and returns an instance of EfRepository.
    /// </summary>
    /// <returns>An <see cref="EfRepository"/>.</returns>
    public EfRepository GetEfRepository()
    {
        if (!_context.TestEntities.Any())
        {
            _context.TestEntities.AddRange(
                new List<TestEntity>
                {
                    new() { Id = Guid.NewGuid(), Name = "Abc" },
                    new() { Id = Guid.NewGuid(), Name = "Def" },
                });
            _context.SaveChanges();
        }

        Context = new AppDbContext(_options);
        return new EfRepository(Context);
    }

    /// <summary>
    /// Seeds data and returns an instance of EfNamedEntityRepository.
    /// </summary>
    /// <returns>An <see cref="EfNamedEntityRepository"/>.</returns>
    public EfNamedEntityRepository GetEfNamedEntityRepository()
    {
        if (!_context.TestNamedEntities.Any())
        {
            _context.TestNamedEntities.AddRange(
                new List<TestNamedEntity>
                {
                    new(Guid.NewGuid(), "Abce"),
                    new(Guid.NewGuid(), "Efgh"),
                });
            _context.SaveChanges();
        }

        Context = new AppDbContext(_options);
        return new EfNamedEntityRepository(Context);
    }

    public void Dispose()
    {
        _context.Dispose();
        Context.Dispose();
    }
}
