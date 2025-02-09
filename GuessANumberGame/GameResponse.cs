namespace GuessANumberGame;

public readonly struct GameResponse
{
    public static readonly GameResponse TooHigh = new GameResponse("TooHigh", "Your guess is too high!");
    public static readonly GameResponse TooLow = new GameResponse("TooLow", "Your guess is too low!");

    private readonly string _name;
    private readonly string _friendlyString;

    private GameResponse(string name, string friendlyString)
    {
        _name = name;
        _friendlyString = friendlyString;
    }

    public override string ToString() => _friendlyString;

    public static GameResponse Invalid(int min, int max)
    {
        return new GameResponse("Invalid", $"Please guess a number between {min} and {max}.");
    }
    //public static GameResponse Invalid = new GameResponse("Invalid", "Invalid guess. Please try again.");

    public static GameResponse Correct(int numberOfGuesses)
    {
        return new GameResponse("Correct", $"Congratulations! You guessed it in {numberOfGuesses}.");
    }

    public static GameResponse Correct()
    {
        return new GameResponse("Correct", "Congratulations! You guessed correctly!");    
    }
    
    public override bool Equals(object obj)
    {
        if (obj is GameResponse other)
        {
            return _name == other._name;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return _name.GetHashCode();
    }
    
    public static bool operator ==(GameResponse a, GameResponse b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(GameResponse a, GameResponse b)
    {
        return !(a == b);
    }
}