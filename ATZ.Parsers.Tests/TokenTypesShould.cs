using NUnit.Framework;

namespace ATZ.Parsers.Tests
{
    [TestFixture]
    public class TokenTypesShould
    {
        private class Letter : ITokenType
        {
            public static readonly Letter A = new Letter();
            public static readonly Letter B = new Letter();
        }

        private class Number : ITokenType
        {
            public static readonly Number One = new Number();
        }

        [Test]
        public void ProvideTheAbilityToDefineDifferentSets()
        {
            var a = Letter.A;
            var one = Number.One;
            Assert.AreNotEqual(a, one);

        }

        [Test]
        public void DifferentiateBetweenTheMembersOfTheSameSet()
        {
            var a = Letter.A;
            var b = Letter.B;
            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void BeAbleToCheckEquality()
        {
            var a1 = Letter.A;
            var a2 = Letter.A;
            Assert.AreEqual(a1, a2);
        }
    }
}
