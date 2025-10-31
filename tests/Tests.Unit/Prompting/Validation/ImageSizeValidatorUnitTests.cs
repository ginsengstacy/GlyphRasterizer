using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using Resources.Messages;
using Tests.Common.Prompting.Validation;

namespace Tests.Unit.Prompting.Validation;

public sealed class ImageSizeValidatorUnitTests : ValidatorTestBase<ImageSizeValidator, int?>
{
    protected override ImageSizeValidator Validator { get; } = new();

    public static IEnumerable<object[]> ValidSizes()
    {
        yield return new object[] { Config.MinImageSize + 1 };
        yield return new object[] { (Config.MinImageSize + Config.MaxImageSize) / 2 };
        yield return new object[] { Config.MaxImageSize - 1 };
    }

    [Fact]
    public void IsValid_Should_ReturnEmptyInputError_When_InputIsNull() =>
        AssertValidationFailure(null, ErrorMessages.EmptyInput);

    [Fact]
    public void IsValid_Should_ReturnSizeTooSmallError_When_InputIsSmallerThanMinimum()
    {
        int tooSmall = Config.MinImageSize - 1;
        string expectedMessage = string.Format(ErrorMessages.SizeTooSmall_FormatString, Config.MinImageSize);
        AssertValidationFailure(tooSmall, expectedMessage);
    }

    [Fact]
    public void IsValid_Should_ReturnSizeTooLargeError_When_InputIsLargerThanMaximum()
    {
        int tooLarge = Config.MaxImageSize + 1;
        string expectedMessage = string.Format(ErrorMessages.SizeTooLarge_FormatString, Config.MaxImageSize);
        AssertValidationFailure(tooLarge, expectedMessage);
    }

    [Theory]
    [MemberData(nameof(ValidSizes))]
    public void IsValid_Should_ReturnTrue_When_InputIsWithinAllowedRange(int validSize) =>
        AssertValidationSuccess(validSize);

    [Fact]
    public void IsValid_Should_ReturnTrue_When_InputEqualsMinimum() =>
        AssertValidationSuccess(Config.MinImageSize);

    [Fact]
    public void IsValid_Should_ReturnTrue_When_InputEqualsMaximum() =>
        AssertValidationSuccess(Config.MaxImageSize);

}
