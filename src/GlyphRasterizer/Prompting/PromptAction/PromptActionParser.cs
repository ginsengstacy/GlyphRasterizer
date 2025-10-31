using GlyphRasterizer.Lookup;
using GlyphRasterizer.Lookup.PromptAction;

namespace GlyphRasterizer.Prompting.PromptAction;

public sealed class PromptActionParser : IPromptInputParser<string, PromptActionType>
{
    public bool TryParse(string input, out PromptActionType value, out string? errorMessage)
    {
        errorMessage = null;

        string trimmedInput = input.Trim();

        return LookupHelpers.TryGetKeyFromRepresentation(
            PromptActionDataLookup.Lookup,
            trimmedInput,
            out value,
            StringComparer.OrdinalIgnoreCase
        );
    }
}
