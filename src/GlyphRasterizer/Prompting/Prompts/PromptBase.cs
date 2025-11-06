using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;

namespace GlyphRasterizer.Prompting.Prompts;

public abstract class PromptBase<TInput, TValue>(CommandTypeParser commandTypeParser) : IPrompt where TInput : notnull
{
    protected abstract string Message { get; }
    protected abstract Func<TInput> GetInput { get; }
    protected abstract IPromptInputParser<TInput, TValue> Parser { get; }
    protected virtual IPromptValueValidator<TValue>? Validator { get; } = null;
    protected virtual Action<SessionContext, TValue>? ValueUpdater { get; } = null;

    protected readonly CommandTypeParser _commandTypeParser = commandTypeParser;

    public object? AdditionalParsingContext;
    public object? AdditionalValidationContext;
    public object[] RuntimeMessageParameters = [];

    private TValue? _previousParsedAndValidValue;

    public PromptResult Execute()
    {
        Console.Write($"{string.Format(Message, RuntimeMessageParameters)} ");
        TInput input = GetInput();

        if (input is string stringInput && _commandTypeParser.TryParse(stringInput, out CommandType? commandType, out _))
        {
            return new PromptResult(commandType!.Value);
        }

        if (!Parser.TryParse(input, out TValue? value, out string? errorMessage, AdditionalParsingContext))
        {
            return Retry(errorMessage!);
        }

        if (Validator is not null && !Validator.IsValid(value!, out errorMessage, AdditionalValidationContext))
        {
            return Retry(errorMessage!);
        }

        _previousParsedAndValidValue = value!;
        return Success(value!);

        PromptResult Success(TValue value) => new(PromptActionType.Continue, value!);
        PromptResult Retry(string message) => new(PromptActionType.Retry, message);
    }

    public void UpdateContextWithParsedAndValidValue(SessionContext context)
    {
        if (_previousParsedAndValidValue is not null)
        {
            ValueUpdater?.Invoke(context, _previousParsedAndValidValue);
        }
    }
}