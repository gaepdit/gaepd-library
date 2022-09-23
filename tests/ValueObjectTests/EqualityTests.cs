using FluentAssertions;
using GaEpd.AppLibrary.Domain.ValueObjects;

namespace GaEpd.AppLibrary.Tests.ValueObjectTests;

public class EqualityTests
{
    private record ValueObjectType() : ValueObject
    {
        public ValueObjectType(string name, int count) : this()
        {
            Name = name;
            Count = count;
        }

        private string Name { get; } = string.Empty;
        private int Count { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Count;
        }
    }

    private const string StringConst = "abc";
    private const int IntConst = 1;

    [Test]
    public void EquivalentValueObjectsAreEqual()
    {
        var objectOne = new ValueObjectType(StringConst, IntConst);
        var objectTwo = new ValueObjectType(StringConst, IntConst);

        objectOne.Equals(objectTwo).Should().BeTrue();
        objectTwo.Equals(objectOne).Should().BeTrue();

        (objectOne == objectTwo).Should().BeTrue();
        (objectTwo == objectOne).Should().BeTrue();

        (objectOne != objectTwo).Should().BeFalse();
        (objectTwo != objectOne).Should().BeFalse();
    }

    [Test]
    public void ValueObjectsAreNotEqualToNull()
    {
        var objectOne = new ValueObjectType(StringConst, IntConst);

        objectOne.Equals(null).Should().BeFalse();

        (objectOne == null).Should().BeFalse();
        (null == objectOne).Should().BeFalse();

        (objectOne != null).Should().BeTrue();
        (null != objectOne).Should().BeTrue();
    }

    [Test]
    public void DifferentValueObjectsAreNotEqualToEachOther()
    {
        var objectOne = new ValueObjectType(StringConst, IntConst);
        var objectTwo = new ValueObjectType(StringConst, IntConst + 1);

        objectOne.Equals(objectTwo).Should().BeFalse();
        objectTwo.Equals(objectOne).Should().BeFalse();

        (objectOne == objectTwo).Should().BeFalse();
        (objectTwo == objectOne).Should().BeFalse();

        (objectOne != objectTwo).Should().BeTrue();
        (objectTwo != objectOne).Should().BeTrue();
    }
}
