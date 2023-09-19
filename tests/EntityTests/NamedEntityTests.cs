using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.EntityTests;

public class NamedEntityTests
{
    [Test]
    public void SetName_ShouldSetTheName_WhenValidNameIsProvided()
    {
        var sut = new DerivedNamedEntity(Guid.NewGuid(), "Test");

        sut.SetName("Changed");

        sut.Name.Should().Be("Changed");
    }

    [Test]
    public void SetName_ShouldThrowException_WhenInvalidNameIsProvided()
    {
        var sut = new DerivedNamedEntity(Guid.NewGuid(), "Test");

        var action = () => sut.SetName(string.Empty);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void SetName_ShouldThrowException_WhenNameIsShorterThanMinimum()
    {
        var sut = new DerivedNamedEntity(Guid.NewGuid(), "Test");

        var action = () => sut.SetName("123");

        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{sut.MinNameLength}'.*");
    }

    [Test]
    public void SetName_ShouldThrowException_WhenNameIsLongerThanMaximum()
    {
        var sut = new DerivedNamedEntity(Guid.NewGuid(), "Test");

        var action = () => sut.SetName("1234567890");

        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length cannot exceed the maximum length '{sut.MaxNameLength}'.*");
    }

    [Test]
    public void NameWithActivity_ShouldAppendInactive_WhenActiveIsFalse()
    {
        var sut = new DerivedNamedEntity(Guid.NewGuid(), "Test") { Active = false };

        var result = sut.NameWithActivity;

        result.Should().Be("Test [Inactive]");
    }

    [Test]
    public void NoMinMax_ShouldSetTheName_WhenValidNameIsProvided()
    {
        var sut = new DerivedNamedEntityNoMinMax(Guid.NewGuid(), "Test");

        sut.SetName("Changed");

        sut.Name.Should().Be("Changed");
    }

    [Test]
    public void NoMinMax_ShouldThrowException_WhenEmptyNameIsProvided()
    {
        var sut = new DerivedNamedEntityNoMinMax(Guid.NewGuid(), "Test");

        var action = () => sut.SetName(string.Empty);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }
}
