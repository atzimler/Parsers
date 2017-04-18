using System;
using JetBrains.Annotations;

namespace ATZ.Parsers
{
    /// <summary>
    /// Representation of a token. This base class represents a token capable of holding one character.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The type of the token.
        /// </summary>
        /// <remarks>
        /// Default value is <see cref="Parsers.TokenType.Unknown"/>
        /// </remarks>
        [NotNull]
        public ITokenType TokenType { get; protected set; }

        /// <summary>
        /// Textual representation of the token as found in the source.
        /// </summary>
        public string Text { get; protected set; }
        /// <summary>
        /// Value assigned to the token.
        /// </summary>
        /// <remarks>
        /// This is an extension point for derived classes.
        /// </remarks>
        public object Value { get; protected set; }

        /// <summary>
        /// Source the token is read from.
        /// </summary>
        /// <remarks>
        /// This is to support different parsing strategies.
        /// </remarks>
        [NotNull]
        protected Source Source { get; }

        /// <summary>
        /// Line number the token was found in the source.
        /// </summary>
        public int LineNumber { get; }
        /// <summary>
        /// Character position in the line number where the token was found in the source.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Constructs a Token object with given <see cref="Source"/> and token type of <see cref="Parsers.TokenType.Unknown"/>.
        /// </summary>
        /// <param name="source">The source of the token.</param>
        /// <remarks>
        /// This constructor is to provide support for tokens capable of containing multiple token types. TokenType can be changed
        /// during the extraction of the token, which is invoked during construction as a last step.
        /// </remarks>
        protected Token([NotNull] Source source)
            : this(source, Parsers.TokenType.Unknown)
        {
        }

        /// <summary>
        /// Constructs a Token object with given <see cref="Source"/> and <see cref="ITokenType"/>.
        /// </summary>
        /// <param name="source">The source of the token.</param>
        /// <param name="tokenType">The type of the token.</param>
        public Token([NotNull] Source source, [NotNull] ITokenType tokenType)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            TokenType = tokenType ?? throw new ArgumentNullException(nameof(tokenType));

            LineNumber = source.LineNumber;
            Position = source.CurrentPosition;

            // ReSharper disable once VirtualMemberCallInConstructor => You should follow LSP when deriving and then it will be ok.
            // This provides opportunity to extract different amount of characters from the source.
            Extract();
        }

        /// <summary>
        /// Extracts the token from the source.
        /// </summary>
        protected virtual void Extract()
        {
            Text = Source.CurrentCharacter.ToString();
            Value = null;

            Source.NextCharacter();
        }
    }
}