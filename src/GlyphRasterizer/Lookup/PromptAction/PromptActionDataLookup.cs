using GlyphRasterizer.Prompting.PromptAction;
using System.Collections.Immutable;

namespace GlyphRasterizer.Lookup.PromptAction;

public static class PromptActionDataLookup
{
    public static readonly ImmutableDictionary<PromptActionType, PromptActionLookupData> Lookup =
        ImmutableDictionary.Create<PromptActionType, PromptActionLookupData>()
            .Add(PromptActionType.GoBack, new PromptActionLookupData("Back", ImmutableHashSet.Create("back", "undo")))
            .Add(PromptActionType.Restart, new PromptActionLookupData("Restart", ImmutableHashSet.Create("restart", "reload")))
            .Add(PromptActionType.Quit, new PromptActionLookupData("Quit", ImmutableHashSet.Create("quit", "exit"))
        );
}
