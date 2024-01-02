using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.EntityHelpers;

public class DerivedNamedEntityManager(INamedEntityRepository<DerivedNamedEntity> repository)
    : NamedEntityManager<DerivedNamedEntity, INamedEntityRepository<DerivedNamedEntity>>(repository);
