using JetBrains.Annotations;
using System;
using System.Linq;
using System.Text;

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

        private void StoreAndAdvance([NotNull] Func<char, bool> characters, [NotNull] Action<char> store)
        {
            while (characters(Source.CurrentCharacter) && Source.CurrentCharacter != Source.Eol &&
                   Source.CurrentCharacter != Source.Eof)
            {
                store(Source.CurrentCharacter);
                Source.NextCharacter();
            }
        }

        private void VerifyCharactersIsNotNull(object characters)
        {
            if (characters == null)
            {
                throw new ArgumentNullException(nameof(characters));
            }
        }

        /// <summary>
        /// Discard characters from the source until a character is found that is not contained in the characters array.
        /// </summary>
        /// <param name="characters">The characters that need to be discarded.</param>
        /// <exception cref="ArgumentNullException">characters is null.</exception>
        protected void Discard([NotNull] char[] characters)
        {
            VerifyCharactersIsNotNull(characters);
            Discard(characters.Contains);
        }

        /// <summary>
        /// Discard characters from the source until a character is found that is not matching the characters function.
        /// </summary>
        /// <param name="characters">Function describing the characteristics of the characters to be discarded.</param>
        /// <exception cref="ArgumentNullException">characters is null.</exception>
        protected void Discard([NotNull] Func<char, bool> characters)
        {
            VerifyCharactersIsNotNull(characters);
            StoreAndAdvance(characters, c => { });
        }

        /// <summary>
        /// Extracts the token from the source.
        /// </summary>
        /// <remarks>
        /// This default implementation extracts one character from the source.
        /// </remarks>
        protected virtual void Extract()
        {
            Text = Source.CurrentCharacter.ToString();

            Source.NextCharacter();
        }

        /// <summary>
        /// Extracts the token from the source.
        /// </summary>
        /// <param name="characters">Array of characters allowed in the token.</param>
        /// <exception cref="ArgumentNullException">characters is null.</exception>
        protected void Extract([NotNull] char[] characters)
        {
            VerifyCharactersIsNotNull(characters);
            Extract(characters.Contains);
        }

        /// <summary>
        /// Extracts the token from the source.
        /// </summary>
        /// <param name="characters">Function describing the characters allowed in the token.</param>
        /// <exception cref="ArgumentNullException">characters is null.</exception>
        protected void Extract([NotNull] Func<char, bool> characters)
        {
            VerifyCharactersIsNotNull(characters);

            var sb = new StringBuilder();
            StoreAndAdvance(characters, c => sb.Append(c));

            Text = sb.ToString();
        }
    }
}