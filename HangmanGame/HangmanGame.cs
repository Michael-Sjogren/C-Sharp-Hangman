using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HangmanGame
{
    public class HangManGame
    {
        ConsoleColor infoColor = ConsoleColor.DarkYellow;
        ConsoleColor errorColor = ConsoleColor.DarkRed;
        ConsoleColor winColor = ConsoleColor.DarkGreen;
        ConsoleColor loseColor = ConsoleColor.Red;
        public bool IsRunning { get; private set; }
        private HangmanGameLogic game;
        public void Run()
        {
            IsRunning = true;
            
            while (IsRunning)
            {
                // choose if you want to keep playing or not
                Console.WriteLine("Welcome to the Hangman game... without the hangman ;).");
                Console.ForegroundColor = infoColor;
                Console.WriteLine("Enter 'play' to play the game or 'quit' to quit.");
                Console.ResetColor();
                var input = GetUserInput();
                Console.Clear();
                if (input == "play")
                {
                    game = new HangmanGameLogic();
                    game.InitializeGame();
                    // win
                    var isGuessCorrect = false;
                    while (!game.IsGameOver && !isGuessCorrect)
                    {
                        PrintTutorial();
                        PrintGuessesLeft();
                        PrintIncorrectLetters();
                        PrintRevealedLetters();
                        var guess = GetGuessFromUser();
                        if (!IsUserInputValid(guess))
                        {
                            continue;
                        }
                        isGuessCorrect = game.MakeGuess(guess);
                    }
                    if (!game.IsGameOver && isGuessCorrect)
                    {
                        Console.ForegroundColor = winColor;
                        PrintRevealedLetters();
                        Console.WriteLine($"You Win! You guessed correctly! The secret word was { game.SecretWord }.");
                    }
                    // lose
                    else
                    {
                        Console.ForegroundColor = loseColor;
                        Console.WriteLine($"Your guess is wrong. You lost.");
                    }
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (input == "quit")
                {
                    Console.Write("Game quit. Thank you for playing!");
                    Quit();
                }
            }
        }
        public void PrintIncorrectLetters()
        {
            Console.WriteLine("List of all incorrect guessed letters:");
            Console.ForegroundColor = loseColor;
            Console.WriteLine(game.IncorrectLetterGuesses);
            Console.ResetColor();
        }
        private void PrintRevealedLetters()
        {
            foreach (var letter in game.RevealedLetters)
            {
                Console.Write($"{letter}   ");
            }
            Console.WriteLine("\n");
        }

        private void PrintGuessesLeft()
        {
            Console.WriteLine();
            Console.WriteLine($"{HangmanGameLogic.MaxGuesses - game.GuessCount} guesses left.");
        }
        
        
        /** Returns true if the guess is invalid and prints the error. **/
        private bool IsUserInputValid(String guess)
        {
            if (guess.Equals(""))
            {
                PrintError("No guess was inputted.");
                return false;
            }
            if (!game.IsAWord(guess))
            {
                PrintError("Incorrect guess. Not a word.");
                return false;
            }
            
            if (guess.Length != 1 && guess.Length != game.SecretWord.Length)
            {
                PrintError("Incorrect guess. The guessed word should have the same length as the secret word.");
                return false;
            }
            return true;
        }
        
        private void PrintError(String message)
        {
            Console.ForegroundColor = errorColor;
            Console.Write("Error: ");
            Console.ResetColor();
            Console.Write(message);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private void PrintTutorial()
        {
            Console.WriteLine("Guess the secret word!");
            Console.ForegroundColor = infoColor;
            Console.WriteLine("Enter a single letter to guess. Or a whole word.\nAll secret words are in Swedish.\nGuessed words must have the same length as secret word to be a valid guess.");
            Console.WriteLine($"Word length is {game.SecretWord.Length}.");
            Console.ResetColor();
        }

        private String GetUserInput()
        {
            try
            {
                var input = Console.ReadLine();
                if (input != null)
                {
                    return input.Trim().ToLower();
                }
                
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(e);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
            }

            return "";
        }
        private String GetGuessFromUser()
        {
            Console.Write("Your guess: ");
            var guess = GetUserInput();
            return guess;
        }
        public void Quit()
        {
            IsRunning = false;
        }

    }
}