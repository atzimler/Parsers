using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ATZ.Parsers.Tests
{
    [TestFixture]
    public class TokenShould
    {
        private class TestTokenType : ITokenType
        {
            [NotNull]
            public static readonly TestTokenType Character = new TestTokenType();
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Token> ReadAllNumbers([NotNull] string str)
        {
            return ReadAllTokensForNumberReading(str).Where(t => t.TokenType == NumberReadingToken.Number);
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Token> ReadAllTokens([NotNull] string str)
        {
            using (var sr = new StringReader(str))
            {
                var s = new Source(sr);
                while (s.CurrentCharacter != Source.Eof)
                {
                    yield return new Token(s, TestTokenType.Character);
                }
            }
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Token> ReadAllTokensForNumberReading([NotNull] string str)
        {
            using (var sr = new StringReader(str))
            {
                var s = new Source(sr);
                while (s.CurrentCharacter != Source.Eof)
                {
                    yield return new TestNumberReadingToken(s);
                }
            }
        }

        [Test]
        public void BeConstructable()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var sr = new StringReader("X"))
                {
                    var s = new Source(sr);
                    // ReSharper disable once ObjectCreationAsStatement => Testing constructor.
                    new Token(s, TestTokenType.Character);
                }
            });
        }

        [Test]
        public void NotAcceptNullSource()
        {
            // ReSharper disable once ObjectCreationAsStatement => Testing constructor.
            // ReSharper disable once AssignNullToNotNullAttribute => Testing reaction to null as parameter.
            var ex = Assert.Throws<ArgumentNullException>(() => new Token(null, TestTokenType.Character));
            Assert.IsNotNull(ex);
            ex.ParamName.Should().Be("source");
        }

        [Test]
        public void NotAcceptNullTokenType()
        {
            var sr = new StringReader("X");
            var s = new Source(sr);
            // ReSharper disable once ObjectCreationAsStatement => Testing constructor.
            // ReSharper disable once AssignNullToNotNullAttribute => Testing reaction to null as parameter.
            var ex = Assert.Throws<ArgumentNullException>(() => new Token(s, null));
            Assert.IsNotNull(ex);
            ex.ParamName.Should().Be("tokenType");
        }

        [Test]
        public void ReturnCorrectTokens()
        {
            var tokens = ReadAllTokens("X").ToList();
            tokens.Count.Should().Be(2);
        }

        [Test]
        public void HaveCorrectTextualValue()
        {
            var tokenTexts = ReadAllTokens("X").Select(t => t.Text).ToList();
            tokenTexts.Should().BeEquivalentTo("X", Source.Eol.ToString());
        }

        [Test]
        public void HaveCorrectPosition()
        {
            var tokenPositions = ReadAllTokens("X").Select(t => t.Position).ToList();
            tokenPositions.Should().BeEquivalentTo(0, 1);
        }

        [Test]
        public void HaveCorrectLineNumber()
        {
            var lineNumbers = ReadAllTokens("X\nY").Select(t => t.LineNumber).ToList();
            lineNumbers.Should().BeEquivalentTo(1, 1, 2, 2);
        }

        [Test]
        public void ProvideAbilityToDefineComplexTokens()
        {
            var numbers = ReadAllNumbers("12345;123;42").ToList();
            // ReSharper disable once PossibleNullReferenceException => ReadAllNumbers is [ItemNotNull] but there are bugs in the R# reasoning engine. (Problem on 2017.1.1)
            numbers.Select(t => t.Value).Should().BeEquivalentTo(12345, 123, 42);
        }

        [Test]
        public void BeAbleToDiscardCharacters()
        {
            using (var sr = new StringReader("  X"))
            {
                var s = new Source(sr);
                var t = new SpaceSkippingToken(s);
                t.Text.Should().Be("X");
            }
        }

        [Test]
        public void BeAbleToDiscardCharactersByFunction()
        {
            using (var sr = new StringReader(" \tX"))
            {
                var s = new Source(sr);
                var t = new WhiteSpaceSkippingToken(s);
                t.Text.Should().Be("X");
            }
        }
    }
}
