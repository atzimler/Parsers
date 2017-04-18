using JetBrains.Annotations;

namespace ATZ.Parsers
{
    public class TokenType : ITokenType
    {
        [NotNull]
        public static readonly TokenType Unknown = new TokenType();
    }
}
