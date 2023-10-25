using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.EntityHelpers;

public class DerivedNamedEntityManager : NamedEntityManager<DerivedNamedEntity, INamedEntityRepository<DerivedNamedEntity>>
{
    public DerivedNamedEntityManager(INamedEntityRepository<DerivedNamedEntity> repository) : base(repository) { }
}
