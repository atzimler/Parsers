using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace ATZ.Parsers.Tests
{
    [TestFixture]
    public class SourceShould
    {
        [Test]
        public void BeConstructable()
        {
            using (var sr = new StringReader("X"))
            {
                // ReSharper disable once ObjectCreationAsStatement => We are testing the constructor against exceptions.
                Assert.DoesNotThrow(() => new Source(sr));
            }
        }

        [Test]
        public void ReturnTheCurrentCharacterOnInitialCallToCurrentCharacter()
        {
            using (var sr = new StringReader("X"))
            {
                var s = new Source(sr);
                s.CurrentCharacter.Should().Be('X');
            }
        }

        [Test]
        public void ReturnEndOfFileOnCurrentCharacterIfTheStreamIsEmpty()
        {
            using (var sr = new StringReader(""))
            {
                var s = new Source(sr);
                s.CurrentCharacter.Should().Be(Source.Eof);
            }
        }

        [Test]
        public void ReturnEndOfLineOnCurrentCharacterIfTheLineIsAtItsEnd()
        {
            using (var sr = new StringReader("\n"))
            {
                var s = new Source(sr);
                s.CurrentCharacter.Should().Be(Source.Eol);
            }
        }

        [Test]
        public void ThrowArgumentNullExceptionWhenPassingNullAsConstructorParameter()
        {
            // ReSharper disable once ObjectCreationAsStatement => We are testing the constructor.
            // ReSharper disable once AssignNullToNotNullAttribute => We are testing that the constructor correctly throws an exception for receiving null parameter.
            var ex = Assert.Throws<ArgumentNullException>(() => new Source(null));
            Assert.IsNotNull(ex);

            ex.ParamName.Should().Be("textReader");
        }

        [Test]
        public void ReturnEndOfFileWhenAdvancingAfterTheEndOfFile()
        {
            using (var sr = new StringReader(""))
            {
                var s = new Source(sr);
                s.NextCharacter();
                s.CurrentCharacter.Should().Be(Source.Eof);
            }
        }

        [Test]
        public void AdvanceLinePositionWhenReadingPastTheEndOfLine()
        {
            using (var sr = new StringReader("\nX"))
            {
                var s = new Source(sr);
                s.CurrentCharacter.Should().Be(Source.Eol);
                s.NextCharacter();
                s.CurrentCharacter.Should().Be('X');
            }
        }

        [Test]
        public void ReturnEndOfFileWhenPeekingEmptyFile()
        {
            using (var sr = new StringReader(""))
            {
                var s = new Source(sr);
                s.PeekCharacter().Should().Be(Source.Eof);
            }
        }

        [Test]
        public void ReturnEndOfLineWhenPeekingAtTheEndOfLine()
        {
            using (var sr = new StringReader("\nX"))
            {
                var s = new Source(sr);
                s.PeekCharacter().Should().Be(Source.Eol);
            }
        }

        [Test]
        public void ReturnNextCharacterWhenPeekingInTheMiddleOfTheLine()
        {
            using (var sr = new StringReader("AB"))
            {
                var s = new Source(sr);
                s.PeekCharacter().Should().Be('B');
            }
        }

        [Test]
        public void ReturnZeroForLineNumberIfSourceIsEmpty()
        {
            using (var sr = new StringReader(""))
            {
                var s = new Source(sr);
                s.LineNumber.Should().Be(0);
            }
        }

        [Test]
        public void ReturnOneForLineNumberOfAnyNonEmptySourceWithoutAdvancingTheCharacter()
        {
            using (var sr = new StringReader("A"))
            {
                var s = new Source(sr);
                s.LineNumber.Should().Be(1);
            }
        }

        [Test]
        public void IncrementLineNumberWhenPassingNewLines()
        {
            using (var sr = new StringReader("\nA"))
            {
                var s = new Source(sr);
                s.LineNumber.Should().Be(1);
                s.NextCharacter();
                s.LineNumber.Should().Be(2);
            }
        }
    }
}
