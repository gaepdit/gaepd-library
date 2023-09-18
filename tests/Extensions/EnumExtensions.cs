using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;

namespace GaEpd.AppLibrary.Tests.Extensions;

[TestFixture]
public class EnumExtensions
{
    [TestCase(EnumTest.DisplayOnly, "Display Only")]
    [TestCase(EnumTest.DisplayAndDescription, "Display for Display And Description")]
    public void GetDisplayName_ReturnsDisplayAttribute(EnumTest input, string output)
    {
        input.GetDisplayName().Should().Be(output);
    }

    [TestCase(EnumTest.None, "None")]
    [TestCase(EnumTest.DescriptionOnly, "DescriptionOnly")]
    public void GivenNoDisplayAttribute_GetDisplayName_ReturnsEnum(EnumTest input, string output)
    {
        input.GetDisplayName().Should().Be(output);
    }

    [TestCase(EnumTest.DescriptionOnly, "Description Only")]
    [TestCase(EnumTest.DisplayAndDescription, "Description for Display And Description")]
    public void GetDescription_ReturnsDescriptionAttribute(EnumTest input, string output)
    {
        input.GetDescription().Should().Be(output);
    }

    [TestCase(EnumTest.None, "None")]
    [TestCase(EnumTest.DisplayOnly, "DisplayOnly")]
    public void GivenNoDescriptionAttribute_GetDescription_ReturnsEnum(EnumTest input, string output)
    {
        input.GetDescription().Should().Be(output);
    }

    public enum EnumTest
    {
        None,

        [Display(Name = "Display Only")] DisplayOnly,

        [System.ComponentModel.Description("Description Only")]
        DescriptionOnly,

        [Display(Name = "Display for Display And Description")]
        [System.ComponentModel.Description("Description for Display And Description")]
        DisplayAndDescription,
    }
}
