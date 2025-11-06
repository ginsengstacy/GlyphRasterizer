using FluentAssertions;
using GlyphRasterizer.Extensions;
using GlyphRasterizer.Prompting;

namespace Tests.Common.Prompting.Validation;

public abstract class ValidatorTestBase<TValidator, TValue> where TValidator : IPromptValueValidator<TValue>
{
    protected abstract TValidator Validator { get; }

    protected void AssertValidationSuccess(TValue value, object? additionalContext = null)
    {
        bool success = Validator.IsValid(value, out string? errorMessage, additionalContext);

        success.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    protected void AssertValidationFailure(TValue value, string expectedMessage, object? additionalContext = null, bool errorMessageShouldMatchFormat = false)
    {
        bool success = Validator.IsValid(value, out string? errorMessage, additionalContext);

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
