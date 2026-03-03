using System.Collections.Generic;

namespace ClassLibrary1
{
    public class WordChecker
    {
        public List<LetterState> CheckGuess(string guess, string target)
        {
            guess = guess.ToUpperInvariant();
            target = target.ToUpperInvariant();
            var result = Enumerable.Repeat(LetterState.Incorrect, guess.Length).ToList();
            var used = new bool[target.Length];
            var tChars = target.ToCharArray();

            for (int i = 0; i < guess.Length; i++)
            {
                if (guess[i] == tChars[i])
                {
                    result[i] = LetterState.CorectLetter;
                    used[i] = true;
                }
            }

            for (int i = 0; i < guess.Length; i++)
            {
                if (result[i] == LetterState.CorectLetter) continue;
                for (int j = 0; j < tChars.Length; j++)
                {
                    if (!used[j] && guess[i] == tChars[j])
                    {
                        result[i] = LetterState.WrongPosition;
                        used[j] = true;
                        break;
                    }
                }
            }

            return result;
        }
    }
}