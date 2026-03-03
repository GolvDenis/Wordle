using System;

namespace ClassLibrary1
{
    public class Game
    {
        private readonly Word wordProvider;
        private readonly WordChecker checker;
        private readonly Board board;
        private readonly IConsoleRenderer renderer;

        public Game(Word wordProvider, WordChecker checker, Board board, IConsoleRenderer renderer)
        {
            this.wordProvider = wordProvider;
            this.checker = checker;
            this.board = board;
            this.renderer = renderer;
        }

        public void Run()
        {
            var target = wordProvider.GenerateTargetWord().ToUpperInvariant();
            // тимчасовий рядок
            Console.WriteLine($"[DEBUG] Загадане слово: {target}");
            Console.ReadKey(true);

            while (board.CanGuess && !board.IsSolved(target))
            {
                renderer.Render(board);
                var input = renderer.ReadGuess(board.WordLength);

                if (string.Equals(input, "EXIT", StringComparison.OrdinalIgnoreCase))
                    break;

                if (input.Length != board.WordLength)
                {
                    renderer.ShowMessage($"Слово має містити {board.WordLength} літер.");
                    continue;
                }

                if (!wordProvider.IsValidWord(input))
                {
                    renderer.ShowMessage("Слово не знайдено в словнику.");
                    continue;
                }

                var states = checker.CheckGuess(input, target);
                var guess = new Guess(input, states);
                board.AddGuess(guess);
            }

            renderer.Render(board);
            renderer.RenderEnd(board.IsSolved(target), target);
        }
    }
}