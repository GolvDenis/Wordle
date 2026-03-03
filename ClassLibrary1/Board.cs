using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Board
    {
        public int MaxAttempts { get; }
        public int WordLength { get; }

        private readonly List<Guess> guesses = new();
        public IReadOnlyList<Guess> Guesses => guesses;

        public Board(int wordLength, int maxAttempts)
        {
            WordLength = wordLength;
            MaxAttempts = maxAttempts;
        }

        public bool CanGuess => guesses.Count < MaxAttempts;

        public void AddGuess(Guess guess)
        {
            if (!CanGuess)
                throw new InvalidOperationException("Більше немає спроб.");

            if (guess.Word.Length != WordLength)
                throw new ArgumentException("Невірна довжина слова.");

            guesses.Add(guess);
        }

        public bool IsSolved(string target)
        {
            return guesses.Any(g => g.Word == target);
        }
    }
}