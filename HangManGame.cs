using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace C_Sharp_Hangman
{
    public class HangManGame
    {
        ConsoleColor infoColor = ConsoleColor.DarkYellow;
        ConsoleColor errorColor = ConsoleColor.DarkRed;
        ConsoleColor winColor = ConsoleColor.DarkGreen;
        ConsoleColor loseColor = ConsoleColor.Red;
        private String[] words = new String[]{"person","jobb","spel","grafik","text","orm","cyckelpump","bössa","paraply","citron","päron","apelsin","tacos","pizza","läsk","studier","jul","ekvation",
"namn","rep","godis","chips","bil","spårvagn","sjukhus","kossa"};
        private int guessCount = 0;
        private bool isGuessCorrect = false;
        private bool lostGame = false;
        private HashSet<char> guessedLetters;
        private char[] secretWordLetters;

        private StringBuilder builder;

        public const int MaxGuesses = 10;
        // letters to write out in the console either a letter or underscore '_'
        public char[] CorrectlyGuessedLetters {get; private set;}
        public bool IsRunning { get; private set; }

        public HangManGame()
        {
            builder = new StringBuilder();
            guessedLetters = new HashSet<char>();
        }

        public void Reset()
        {
            var rng = new Random();
            guessedLetters.Clear();
            builder.Clear();
            secretWordLetters = words[rng.Next(0, words.Length)].ToCharArray();
            CorrectlyGuessedLetters = new char[secretWordLetters.Length];
            lostGame = false;
            isGuessCorrect = false;
            guessCount = 0;
        }
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

                    MakeGuesses();
                    // win
                    if (isGuessCorrect && !lostGame)
                    {
                        Console.ForegroundColor = winColor;
                        Console.WriteLine($"You Win! You guessed correctly! The secret word was { new String(secretWordLetters) }.");
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
        private void PrintGuessesLeft()
        {
            Console.WriteLine();
            Console.WriteLine($"{MaxGuesses - guessCount} guesses left.");
        }

        private void PrintIncorrectGuessedLetters()
        {
            if (builder.Length > 0)
            {
                Console.WriteLine("List of incorrect letter guesses:");
                Console.ForegroundColor = loseColor;
                Console.WriteLine(builder);
                Console.ResetColor();
            }
        }

        private void PrintRevealedLettersAndDashes()
        {
            for (int i = 0; i < secretWordLetters.Length; i++)
            {
                char letter = secretWordLetters[i];
                bool hasSeenLetter = guessedLetters.Contains(letter) || isGuessCorrect;
                CorrectlyGuessedLetters[i] = hasSeenLetter ? letter : '_';
                Console.Write($"{CorrectlyGuessedLetters[i]}   ");
            }
            Console.WriteLine();
        }

        private void MakeGuesses()
        {
            Reset();

            while (!lostGame && !isGuessCorrect)
            {
                lostGame = guessCount >= MaxGuesses;
                if (lostGame)
                {
                    break;
                }
                Console.Clear();
                PrintTutorial();
                PrintIncorrectGuessedLetters();
                PrintGuessesLeft();
                PrintRevealedLettersAndDashes();
                String guess = GetGuessFromUser();
                
                bool isSingleLetterGuess = guess.Length == 1;
                bool isWordGuess = guess.Length == secretWordLetters.Length;
                bool isEmpty = guess.Length == 0;

                if (IsNotUserInputValid(isSingleLetterGuess , isWordGuess , isEmpty))
                {
                    continue;
                }

                var stringComparer = StringComparison.OrdinalIgnoreCase;
                // exact correct word guess
                var secretWord = new String(secretWordLetters);
                if (guess.Equals(secretWord, stringComparer))
                {
                    isGuessCorrect = true;
                }
                else if (!guess.Equals(secretWord, stringComparer) && isWordGuess)
                {
                    guessCount++;
                }
                // single letter guesses
                else if (isSingleLetterGuess)
                {
                    char letterGuess = guess[0];
                    bool alreadyGuessed = guessedLetters.Contains(letterGuess);
                    bool isLetterCorrect = secretWordLetters.Any(e => e == letterGuess);
                    if (isLetterCorrect && !alreadyGuessed)
                    {
                        guessedLetters.Add(letterGuess);
                        guessCount++;
                    }
                    else if(!alreadyGuessed)
                    {
                        builder.Append(letterGuess + ", ");
                        guessedLetters.Add(letterGuess);
                        guessCount++;
                    }
                }
                // if guessed letters cotains all correct letters
                if (secretWordLetters.All(e => guessedLetters.Contains(e)))
                {
                    isGuessCorrect = true;
                }
            }
        }

        /** Returns true if the guess is invalid and prints the error. **/
        private bool IsNotUserInputValid(bool isSingleLetter , bool isWordGuess , bool isEmpty)
        {
            var incorrectWord = !isSingleLetter && !isWordGuess;
            if (isEmpty)
            {
                PrintError("No guess was inputted.");
            }
            else if(incorrectWord)
            {
                PrintError("Incorrect guess. The guessed word should have the same length as the secret word.");
            }
            return incorrectWord || isEmpty;
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
            Console.WriteLine($"Word length is {secretWordLetters.Length}.");
            Console.ResetColor();
        }

        private String GetUserInput()
        {
            try
            {
                var input = Console.ReadLine();
                if (input != null)
                {
                    return  input.Trim().ToLower();
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