using System;
using System.Linq;
using System.Collections.Generic;

namespace C_Sharp_Hangman
{
    public class HangManGame
    {
        private const int MaxGuesses = 10;
        private string word = "kossa";
        public bool IsRunning { get; private set; }
        public void Run()
        {
            IsRunning = true;
            while (IsRunning)
            {
                if(MakeGuess())
                {
                    Console.WriteLine("You guessed right!");
                }
                else
                {
                    Console.WriteLine("You lost!");
                }
            }
        }

        private void PrintGuessedLetters(Dictionary<String , bool> guessedLetters)
        {

            foreach(var letter in word)
            {
                var l = letter.ToString();
                if(guessedLetters[l])
                {
                    Console.Write($"{l}   ");
                }
                else
                {
                    Console.Write("_   ");
                }
            }
            Console.WriteLine();
        }
        private bool MakeGuess()
        {
            int guessCount = 1;
            var won = false;
            var guessedLetters = new Dictionary<String , bool >();
            var guessedWords = new HashSet<String>();
            // fill the dictonary with the correct letters the application chooses
            foreach(var letter in word)
            {
                var l = letter.ToString();
                guessedLetters[l] = false;
                
            }

            while(guessCount <= MaxGuesses && !won)
            {

                String guess = GetGuessFromUser();
                // exact correct word guess
                if (guess.Equals(word))
                {
                    won = true;
                }
                else if(!guess.Equals(word) && guess.Length > 1)
                {

                }
                
                // single letter guesses
                if(guess.Length == 1)
                {
                    if(guessedLetters.ContainsKey(guess))
                    {
                        var alreadyGuessed = guessedLetters[guess];
                        if (!alreadyGuessed)
                        {
                            guessedLetters[guess] = true;
                        }
                    }
                    else
                    {
                        guessCount++;
                    }
                }

                if(guessedLetters.Values.All( value => value == true))
                {
                    won = true;
                }
                PrintGuessedLetters(guessedLetters);
            }
            return won;
        }

        private String GetGuessFromUser()
        {
            Console.Write("Enter your guess: ");
            var guess = Console.ReadLine().Trim().ToLower();
            return guess;
        }
        public void Stop()
        {
            IsRunning = false;
        }
        
    }
}