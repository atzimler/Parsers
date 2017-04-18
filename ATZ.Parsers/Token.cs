using System;
using JetBrains.Annotations;

namespace ATZ.Parsers
{
    public class Token
    {
        [NotNull]
        public ITokenType TokenType { get; protected set; }

        public string Text { get; protected set; }
        public object Value { get; protected set; }

        [NotNull]
        protected Source Source { get; }

        public int LineNumber { get; }
        public int Position { get; }

        public Token([NotNull] Source source)
            : this(source, Parsers.TokenType.Unknown)
        {
        }

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

        protected virtual void Extract()
        {
            Text = Source.CurrentCharacter.ToString();
            Value = null;

            Source.NextCharacter();
        }
    }
}