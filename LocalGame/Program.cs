using GuessANumberGame;

namespace LocalGame;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Game game = new Game();
        int userGuess;
        do
        {
            Console.WriteLine("Input a number, please.");
            string? input = Console.ReadLine();
            userGuess = Convert.ToInt32(input);
            game.Guess(userGuess);
        } while (game.IsRunning);
    }
}