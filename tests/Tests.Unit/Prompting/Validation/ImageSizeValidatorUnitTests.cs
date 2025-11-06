using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.Prompts.InputType.String.ImageSize;
using ImageMagick;
using Resources.Messages;
using System.Collections.Immutable;
using Tests.Common.Prompting.Validation;

namespace Tests.Unit.Prompting.Validation;

public sealed class ImageSizeValidatorUnitTests : ValidatorTestBase<ImageSizeValidator, uint?>
{
    protected override ImageSizeValidator Validator { get; } = new();

    [Fact]
    public void IsValid_Should_ReturnEmptyInputError_When_InputIsNull() => AssertValidationFailure(null, ErrorMessages.EmptyInput);

    [Fact]
    public void IsValid_Should_ReturnSizeTooSmallError_When_InputIsSmallerThanMinimumSize_AndFormatIsNotIco()
    {
        uint tooSmall = AppConfig.MinImageSize - 1;
        string expectedMessage = string.Format(ErrorMessages.SizeTooSmall_FormatString, AppConfig.MinImageSize);
        AssertValidationFailure(tooSmall, expectedMessage, ImmutableArray.Create(MagickFormat.Png));
    }

    [Fact]
    public void IsValid_Should_ReturnSizeTooLargeError_When_InputIsLargerThanMaximumSize_AndFormatIsNotIco()
    {
        uint tooLarge = AppConfig.MaxImageSize + 1;
        string expectedMessage = string.Format(ErrorMessages.SizeTooLarge_FormatString, AppConfig.MaxImageSize);
        AssertValidationFailure(tooLarge, expectedMessage, ImmutableArray.Create(MagickFormat.Png));
    }

    [Fact]
    public void IsValid_Should_ReturnSizeTooLargeForIcoError_When_InputIsLargerThanMaximumIcoSize_AndFormatIsIco()
    {
        uint tooLarge = AppConfig.MaxIcoSize + 1;
        string expectedMessage = string.Format(ErrorMessages.SizeTooLargeForIco_FormatString, AppConfig.MaxIcoSize);
        AssertValidationFailure(tooLarge, expectedMessage, ImmutableArray.Create(MagickFormat.Ico));
    }

    [Fact]
    public void IsValid_Should_ReturnTrue_When_InputEqualsMinimumSize_AndFormatIsNotIco() =>
        AssertValidationSuccess(AppConfig.MinImageSize, ImmutableArray.Create(MagickFormat.Png));

    [Fact]
    public void IsValid_Should_ReturnTrue_When_InputEqualsMinimumSize_AndFormatIsIco() =>
    AssertValidationSuccess(AppConfig.MinImageSize, ImmutableArray.Create(MagickFormat.Ico));

    [Fact]
    public void IsValid_Should_ReturnTrue_When_InputEqualsMaximumSize_AndFormatIsNotIco() =>
        AssertValidationSuccess(AppConfig.MaxImageSize, ImmutableArray.Create(MagickFormat.Png));

    [Fact]
    public void IsValid_Should_ReturnTrue_When_InputEqualsMaximumIcoSize_AndFormatIsIco() =>
        AssertValidationSuccess(AppConfig.MaxIcoSize, ImmutableArray.Create(MagickFormat.Ico));
}
