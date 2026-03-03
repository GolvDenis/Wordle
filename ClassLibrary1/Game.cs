using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Game
    {
        private readonly Word wordProvider;
        private readonly WordChecker checker;
        private readonly Board board;

        public Game(Word wordProvider, WordChecker checker, Board board)
        {
            this.wordProvider = wordProvider;
            this.checker = checker;
            this.board = board;
        }

        public void Run()
        {
            var target = wordProvider.GenerateTargetWord().ToUpperInvariant();

            while (board.CanGuess && !board.IsSolved(target))
            {
                Render();
                Console.Write("Введіть слово: ");
                var input = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

                if (input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    break;

                if (input.Length != board.WordLength)
                {
                    Console.WriteLine($"Слово має містити {board.WordLength} літер.");
                    Console.ReadKey(true);
                    continue;
                }

                if (!wordProvider.IsValidWord(input))
                {
                    Console.WriteLine("Слово не знайдено в словнику.");
                    Console.ReadKey(true);
                    continue;
                }

                var states = checker.CheckGuess(input, target);
                var guess = new Guess(input, states);

                board.AddGuess(guess);
            }

            Render();

            if (board.IsSolved(target))
                Console.WriteLine($"\nВітаю! Ви вгадали слово: {target}");
            else
                Console.WriteLine($"\nГру завершено. Загадане слово: {target}");
        }

        private void Render()
        {
            Console.Clear();
            Console.WriteLine($"Wordle — {board.Guesses.Count}/{board.MaxAttempts}\n");

            for (int r = 0; r < board.MaxAttempts; r++)
            {
                if (r < board.Guesses.Count)
                    DrawGuess(board.Guesses[r]);
                else
                    DrawEmptyRow();

                Console.WriteLine();
            }

            Console.WriteLine("\nЩоб вийти — введіть EXIT.");
        }

        private void DrawGuess(Guess guess)
        {
            for (int i = 0; i < guess.Word.Length; i++)
            {
                SetConsoleColors(guess.States[i]);
                Console.Write($" {guess.Word[i]} ");
                Console.ResetColor();
            }
        }

        private void DrawEmptyRow()
        {
            for (int i = 0; i < board.WordLength; i++)
                Console.Write(" _ ");
        }

        private void SetConsoleColors(LetterState s)
        {
            if (s == LetterState.CorectLetter)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (s == LetterState.WrongPosition)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}