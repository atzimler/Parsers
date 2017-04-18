using System;
using System.IO;
using JetBrains.Annotations;

namespace ATZ.Parsers
{
    /// <summary>
    /// Representation of a source to process.
    /// </summary>
    public class Source
    {
        /// <summary>
        /// Character representing the line ends.
        /// </summary>
        public const char Eol = '\n';

        /// <summary>
        /// Character representing the file end.
        /// </summary>
        public const char Eof = (char)0;

        [NotNull]
        private readonly TextReader _textReader;
        private string _line;

        /// <summary>
        /// Current position on the currently processed line.
        /// </summary>
        public int CurrentPosition { get; private set; } = -2;

        /// <summary>
        /// Current character in the processed source.
        /// </summary>
        public char CurrentCharacter { get; private set; }

        /// <summary>
        /// Current line position in the processed file.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Constructs a Source object.
        /// </summary>
        /// <param name="textReader">Input stream to read the input from.</param>
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

        /// <summary>
        /// Step the source to the next character.
        /// </summary>
        public void NextCharacter()
        {
            IncrementCurrentPosition();
            ReadCurrentCharacter();
        }

        /// <summary>
        /// Peek the next character in the source without moving the current position.
        /// </summary>
        /// <returns></returns>
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
