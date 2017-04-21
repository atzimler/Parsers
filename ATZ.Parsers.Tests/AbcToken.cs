using JetBrains.Annotations;
using System.Linq;

namespace ATZ.Parsers.Tests
{
    public class AbcToken : Token
    {
        public AbcToken([NotNull] Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            Extract(c => new[] { 'a', 'b', 'c' }.Contains(c));
        }
    }
}
