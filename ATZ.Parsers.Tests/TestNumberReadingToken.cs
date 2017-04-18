using System.Text;
using JetBrains.Annotations;

namespace ATZ.Parsers.Tests
{
    public class TestNumberReadingToken : Token
    {
        public TestNumberReadingToken([NotNull] Source source)
            : base(source)
        {
        }

        protected override void Extract()
        {
            if (!char.IsDigit(Source.CurrentCharacter))
            {
                Source.NextCharacter();
                TokenType = NumberReadingToken.EverythingElse;
                return;
            }

            var sb = new StringBuilder();
            while (char.IsDigit(Source.CurrentCharacter))
            {
                sb.Append(Source.CurrentCharacter);
                Source.NextCharacter();
            }

            Text = sb.ToString();
            Value = int.Parse(Text);
            TokenType = NumberReadingToken.Number;
        }
    }
}
