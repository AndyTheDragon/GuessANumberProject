using GuessANumberGame;
using Xunit;

namespace GuessANumberGame.Tests
{
    public class GameTests
    {
        [Fact]
        public void Guess_ReturnsTooLow_WhenGuessIsBelowTarget()
        {
            // Arrange
            var game = new Game(1, 10);
            typeof(Game).GetField("_targetNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(game, 5); // Set target number to 5 for testing

            // Act
            var result = game.Guess(3);

            // Assert
            Assert.Equal("Too low!", result);
        }

        [Fact]
        public void Guess_ReturnsTooHigh_WhenGuessIsAboveTarget()
        {
            // Arrange
            var game = new Game(1, 10);
            typeof(Game).GetField("_targetNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(game, 5); // Set target number to 5 for testing

            // Act
            var result = game.Guess(7);

            // Assert
            Assert.Equal("Too high!", result);
        }

        [Fact]
        public void Guess_ReturnsCorrectMessage_WhenGuessMatchesTarget()
        {
            // Arrange
            var game = new Game(1, 10);
            typeof(Game).GetField("_targetNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(game, 5); // Set target number to 5 for testing

            // Act
            var firstGuessResult = game.Guess(3); // Increment guess counter
            var secondGuessResult = game.Guess(5); // Correct guess on the second attempt

            // Assert
            Assert.Contains("Correct, you guessed it in 2.", secondGuessResult); // Verify dynamic success message
        }


        [Fact]
        public void Guess_ReturnsValidationMessage_WhenGuessIsOutOfRange()
        {
            // Arrange
            var game = new Game(1, 10);

            // Act
            var result = game.Guess(15);

            // Assert
            Assert.Equal("Please guess a number between 1 and 10.", result);
        }
    }
}