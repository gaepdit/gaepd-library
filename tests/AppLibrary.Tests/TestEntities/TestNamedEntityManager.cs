using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.TestEntities;

public class TestNamedEntityManager(INamedEntityRepository<TestNamedEntity> repository)
    : NamedEntityManager<TestNamedEntity, INamedEntityRepository<TestNamedEntity>>(repository);
