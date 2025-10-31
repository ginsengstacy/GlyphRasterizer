using FluentAssertions;
using GlyphRasterizer.Extensions;
using GlyphRasterizer.Prompting;

namespace Tests.Common.Prompting.Validation;

public abstract class ValidatorTestBase<TValidator, TValue> where TValidator : IPromptValueValidator<TValue>
{
    protected abstract TValidator Validator { get; }

    protected void AssertValidationSuccess(TValue value)
    {
        bool success = Validator.IsValid(value, out string? errorMessage);

        success.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    protected void AssertValidationFailure(TValue value, string expectedMessage, bool errorMessageShouldMatchFormat = false)
    {
        bool success = Validator.IsValid(value, out string? errorMessage);

        success.Should().BeFalse();

        if (errorMessageShouldMatchFormat)
        {
            errorMessage!.MatchesFormat(expectedMessage).Should().BeTrue();
        }
        else
        {
            errorMessage.Should().Be(expectedMessage);
        }
    }
}
