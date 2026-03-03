using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class ConsoleRenderer : IConsoleRenderer
    {
        public void Render(Board board)
        {
            Console.Clear();
            Console.WriteLine($"Wordle — {board.Guesses.Count}/{board.MaxAttempts}\n");

            for (int r = 0; r < board.MaxAttempts; r++)
            {
                if (r < board.Guesses.Count)
                    DrawGuess(board.Guesses[r]);
                else
                    DrawEmptyRow(board.WordLength);

                Console.WriteLine();
            }

            Console.WriteLine("\nЩоб вийти — введіть EXIT.");
        }

        public string ReadGuess(int wordLength)
        {
            Console.Write($"\nВведіть слово ({wordLength} літер) або EXIT: ");
            var input = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();
            return input;
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Натисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey(true);
        }

        public void RenderEnd(bool won, string target)
        {
            Console.WriteLine();
            if (won)
                Console.WriteLine($"Вітаю! Ви вгадали слово: {target}");
            else
                Console.WriteLine($"Гру завершено. Загадане слово: {target}");
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

        private void DrawEmptyRow(int wordLength)
        {
            for (int i = 0; i < wordLength; i++)
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
