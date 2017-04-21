using JetBrains.Annotations;

namespace ATZ.Parsers.Tests
{
    public class AbcToken2 : Token
    {
        public AbcToken2([NotNull] Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            Extract(new[] { 'a', 'b', 'c' });
        }
    }
}