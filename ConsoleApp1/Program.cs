using ClassLibrary1;
using System.Text;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        try
        {
            var wordProvider = new Word("Words.txt");
            var checker = new WordChecker();
            var board = new Board(maxAttempts: 6, wordLength: 5);
            var game = new Game(wordProvider, checker, board);
            game.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }
}