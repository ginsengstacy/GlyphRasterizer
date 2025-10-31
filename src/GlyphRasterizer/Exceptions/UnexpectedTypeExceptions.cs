using Resources.Messages;

namespace GlyphRasterizer.Exceptions;

internal sealed class UnexpectedTypeException(Type? expectedType, Type? actualType) : Exception(FormatMessage(expectedType, actualType))
{
    private static string FormatMessage(Type? expectedType, Type? actualType)
    {
        var expectedTypeName = expectedType?.Name ?? "null";
        var actualTypeName = actualType?.Name ?? "null";
        return string.Format(ExceptionMessages.UnexpectedType_FormatString, expectedTypeName, actualTypeName);
    }
}