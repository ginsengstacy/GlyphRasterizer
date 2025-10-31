using GlyphRasterizer.Configuration;
using GlyphRasterizer.Prompting.PromptAction;

namespace GlyphRasterizer.Prompting.Prompts;

public abstract class PromptBase<TInput, TValue>(PromptActionParser promptActionParser) : IPrompt where TInput : notnull
{
    protected abstract string Message { get; }
    protected abstract Func<TInput> GetInput { get; }
    protected abstract IPromptInputParser<TInput, TValue> Parser { get; }
    protected virtual IPromptValueValidator<TValue>? Validator { get; } = null;
    protected virtual Action<SessionContext, TValue>? ValueUpdater { get; } = null;

    protected readonly PromptActionParser _promptActionParser = promptActionParser;

    public object[] RuntimeMessageParameters = [];

    private TValue? _lastParsedAndValidValue;

    public PromptResult Execute()
    {
        Console.Write($"{string.Format(Message, RuntimeMessageParameters)} ");
        TInput rawInput = GetInput();

        if (rawInput is string stringInput && _promptActionParser.TryParse(stringInput, out PromptActionType actionType, out _))
            return new PromptResult(actionType);

        if (!Parser.TryParse(rawInput, out TValue? value, out string? errorMessage))
            return Retry(errorMessage!);

        if (Validator is not null && !Validator.IsValid(value!, out errorMessage))
            return Retry(errorMessage!);

        _lastParsedAndValidValue = value!;
        return Success(value!);

        PromptResult Success(TValue value) => new(PromptActionType.Continue, value!);
        PromptResult Retry(string message) => new(PromptActionType.Retry, message);
    }

    public void UpdateContextWithParsedAndValidValue(SessionContext context)
    {
        if (_lastParsedAndValidValue is not null)
            ValueUpdater?.Invoke(context, _lastParsedAndValidValue);
    }
}