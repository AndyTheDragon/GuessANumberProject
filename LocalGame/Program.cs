using GuessANumberGame;

namespace LocalGame;

internal class Program
{
    private static string _exitCommand = "q";
    
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var game = new Game();
        int userGuess;
        do
        {
            userGuess = GetUserGuess();
            if (userGuess.Equals(Int32.MinValue)) break;
            Console.WriteLine(game.Guess(userGuess).ToString());
        } while (game.IsRunning);
    }

    private static int GetUserGuess()
    {
        try
        {
            Console.WriteLine("Input a number, please.");
            var input = Console.ReadLine();
            
            if (input != null && input.Trim().ToLower() == _exitCommand) return Int32.MinValue;
            if (int.TryParse(input, out int userGuess)) return userGuess;

            Console.WriteLine("Invalid input. Please enter a valid number.");
            return GetUserGuess();
        }
        catch (IOException e)
        {
            Console.WriteLine($"IOException: {e.Message}");
            return Int32.MinValue;;
        }
        catch (Exception e)
        {
            Console.WriteLine("An unexpected error occurred. Disconnecting...");
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }

    }
}