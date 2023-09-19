﻿using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Get : LocalRepositoryTestBase
{
    [Test]
    public void GetAsync_WhenEntityDoesNotExist_ThrowsException()
    {
        var id = Guid.NewGuid();

        var func = async () => await Repository.GetAsync(id);

        func.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage($"Entity not found. Entity type: {typeof(DerivedEntity).FullName}, id: {id}");
    }

    [Test]
    public async Task GetAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = Repository.Items.First();

        var result = await Repository.GetAsync(entity.Id);

        result.Should().Be(entity);
    }
}
