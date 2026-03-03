using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Guess
    {
        public string Word;
        public IReadOnlyList<LetterState> States;

        public Guess(string word, IReadOnlyList<LetterState> states)
        {
            Word = word.ToUpperInvariant();
            States = states;
        }
    }
}