using JetBrains.Annotations;

namespace ATZ.Parsers.Tests
{
    public class NumberReadingToken : ITokenType
    {
        [NotNull]
        public static readonly NumberReadingToken EverythingElse = new NumberReadingToken();

        [NotNull]
        public static readonly NumberReadingToken Number = new NumberReadingToken();
    }
}
