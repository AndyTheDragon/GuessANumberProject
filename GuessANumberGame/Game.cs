namespace GuessANumberGame;

public class Game
{
    private readonly int _max;
    private readonly int _min;
    private int _targetNumber;

    public Game(int min = 1, int max = 100)
    {
        _min = min;
        _max = max;
        _targetNumber = GenerateNumber(_min, _max);
        NumberOfGuesses = 0;
        IsRunning = true;
    }
    
    public bool IsRunning { get; private set; }
    public int LastGuess { get; private set; }
    public int NumberOfGuesses { get; private set; }

    private int GenerateNumber(int min = 1, int max = 100)
    {
        return new Random().Next(min, max + 1);
    }

    public GameResponse Guess(int guessedNumber)
    {
        NumberOfGuesses++;
        if (guessedNumber < _min || guessedNumber > _max)
        {
            return GameResponse.Invalid(_min, _max);
        }
        
        LastGuess = guessedNumber;
        if (guessedNumber > _targetNumber)
        {
            return GameResponse.TooHigh;
        }

        if (guessedNumber < _targetNumber)
        {
            return GameResponse.TooLow;
        }

        GameResponse output = GameResponse.Correct(NumberOfGuesses);
        NumberOfGuesses = 0;
        _targetNumber = GenerateNumber(_min, _max);
        return output;
    }
    
}