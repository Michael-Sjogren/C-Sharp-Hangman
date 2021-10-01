using System;
using HangmanGame;
using Xunit;

namespace UnitTests
{
    public class HangmanGameLogicTests
    {
        private readonly HangmanGameLogic testSubject;
        
        public HangmanGameLogicTests()
        {
            testSubject = new HangmanGameLogic();
        }
        [Fact]
        public void IsWordGuessValid()
        {
            // Arrange
            testSubject.InitializeGame();
            testSubject.SecretWord = "kossa";
            // Act
            var isGuessCorrect = testSubject.MakeGuess("kossa");
            // Assert
             Assert.True(isGuessCorrect);
        }

        [Fact]
        public void GuessCountIncrementsOnWrongWordGuess()
        {
            testSubject.InitializeGame();
            testSubject.SecretWord = "kossa";
            
            var isGuessCorrect = testSubject.MakeGuess("bossa");

            Assert.False(isGuessCorrect);
            Assert.Equal(1, testSubject.GuessCount);
        }
        
        [Theory]
        [InlineData("L")]
        [InlineData("f")]
        [InlineData("b")]
        [InlineData("d")]
        [InlineData("å")]
        [InlineData("ä")]
        [InlineData("ö")]
        [InlineData("!")]
        [InlineData("^")]
        [InlineData("@")]
        public void GuessCountIncrementsOnWrongLetterGuess(string letterGuess)
        {
            testSubject.InitializeGame();
            testSubject.SecretWord = "kossa";
            
            var isGuessCorrect = testSubject.MakeGuess(letterGuess);

            Assert.False(isGuessCorrect);
            Assert.Equal(1, testSubject.GuessCount);
        }


        [Fact]
        public void NonLettersInGuessIsNotValid()
        {
            testSubject.InitializeGame();
            testSubject.SecretWord = "kossa";
            
            Assert.False(testSubject.MakeGuess("1"));
            Assert.False(testSubject.MakeGuess("k1osa"));
            
            Assert.Equal(0,testSubject.GuessCount);
        }
        [Fact]
        public void GameOverWhenGuessCountIsMaxGuessCount()
        {
            testSubject.InitializeGame();
            testSubject.SecretWord = "kossa";
            
            testSubject.MakeGuess("l");
            testSubject.MakeGuess("m");
            testSubject.MakeGuess("n");
            testSubject.MakeGuess("o");
            testSubject.MakeGuess("p");
            testSubject.MakeGuess("q");
            testSubject.MakeGuess("r");
            testSubject.MakeGuess("bossa");
            testSubject.MakeGuess("å");
            testSubject.MakeGuess("w");
            Assert.Equal(10,testSubject.GuessCount);
            Assert.True(testSubject.IsGameOver);

        }
    }
}