using JetBrains.Annotations;

namespace ATZ.Parsers.Tests
{
    public class SpaceSkippingToken : Token
    {
        public SpaceSkippingToken([NotNull] Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            Discard(new[] { ' ' });
            base.Extract();
        }
    }
}
