using System;
using JetBrains.Annotations;

namespace ATZ.Parsers.Tests
{
    public class WhiteSpaceSkippingToken : Token
    {
        public WhiteSpaceSkippingToken([NotNull] Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            Discard(char.IsWhiteSpace);
            base.Extract();
        }
    }
}