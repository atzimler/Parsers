using System;
using System.IO;
using JetBrains.Annotations;

namespace ATZ.Parsers
{
    public class Source
    {
        public const char Eol = '\n';
        public const char Eof = (char)0;

        [NotNull]
        private readonly TextReader _textReader;
        private string _line;

        public int CurrentPosition { get; private set; } = -2;
        public char CurrentCharacter { get; private set; }
        public int LineNumber { get; private set; } = 0;

        public Source([NotNull] TextReader textReader)
        {
            _textReader = textReader ?? throw new ArgumentNullException(nameof(textReader));
            ReadNextLine();
            NextCharacter();
        }

        private void IncrementCurrentPosition()
        {
            ++CurrentPosition;
            if (_line == null || CurrentPosition <= _line.Length)
            {
                return;
            }

            ReadNextLine();
            ++CurrentPosition;
        }

        private void ReadCurrentCharacter()
        {
            if (_line == null)
            {
                CurrentCharacter = Eof;
            }
            else if (CurrentPosition == -1 || CurrentPosition == _line.Length)
            {
                CurrentCharacter = Eol;
            }
            else
            {
                CurrentCharacter = _line[CurrentPosition];
            }
        }

        private void ReadNextLine()
        {
            _line = _textReader.ReadLine();
            CurrentPosition = -1;
            if (_line != null)
            {
                ++LineNumber;
            }
        }

        public void NextCharacter()
        {
            IncrementCurrentPosition();
            ReadCurrentCharacter();
        }

        public char PeekCharacter()
        {
            if (_line == null)
            {
                return Eof;
            }
            int nextPos = CurrentPosition + 1;
            return nextPos < _line.Length ? _line[nextPos] : Eol;
        }

    }
}
