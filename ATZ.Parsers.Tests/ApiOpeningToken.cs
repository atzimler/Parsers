using System;
using JetBrains.Annotations;

namespace ATZ.Parsers.Tests
{
    public class ApiOpeningToken : Token
    {
        public ApiOpeningToken([NotNull] Source source) : base(source)
        {
        }

        public new void Discard(char[] characters)
        {
            // ReSharper disable once AssignNullToNotNullAttribute => We want to be able to directly address the base function with any parameters for testing only.
            base.Discard(characters);
        }

        public new void Discard(Func<char, bool> characters)
        {
            // ReSharper disable once AssignNullToNotNullAttribute => We want to be able to directly address the base function with any parameters for testing only.
            base.Discard(characters);
        }
    }
}
