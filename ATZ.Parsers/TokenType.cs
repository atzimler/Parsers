using JetBrains.Annotations;

namespace ATZ.Parsers
{
    /// <summary>
    /// Generic token types.
    /// </summary>
    public class TokenType : ITokenType
    {
        /// <summary>
        /// The token type is unknown.
        /// </summary>
        [NotNull]
        public static readonly TokenType Unknown = new TokenType();
    }
}
