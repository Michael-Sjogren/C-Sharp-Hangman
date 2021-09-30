using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HangmanGame
{
    public class HangmanGameLogic
    {
        private HashSet<char> allGuessedLetters;
        private StringBuilder incorrectLetterGuesses; 
        public const int MaxGuesses = 10;
        private char[] correctlyGuessedLetters;
        private String[] words = new String[]{"person","jobb","spel","grafik","text","orm","cyckelpump","bössa","paraply","citron","päron","apelsin","tacos","pizza","läsk","studier","jul","ekvation",
"namn","rep","godis","chips","bil","spårvagn","sjukhus","kossa"};
        public String SecretWord {get; set;}
        public int GuessCount {get; private set;}
        public String CurrentGuess {get;set;}
        public bool IsGameOver => GuessCount >= MaxGuesses;
        public HangmanGameLogic()
        {
            incorrectLetterGuesses = new StringBuilder();
            allGuessedLetters = new HashSet<char>(29);
        }

        private void DetermineAndSetCorrectlyGuessedLetters()
        {
            bool isWordGuessCorrect = IsWordGuessCorrect(CurrentGuess);
            for (int i = 0; i < SecretWord.Length; i++)
            {
                char letter = SecretWord[i];
                bool hasSeenLetter = allGuessedLetters.Contains(letter) || isWordGuessCorrect;
                correctlyGuessedLetters[i] = hasSeenLetter ? letter : '_';
            }
        }

        public bool IsAWord(string text)
        {
            var regex = new Regex(@"^[A-Öa-ö]+$");
            var match = regex.Match(text);
            return match.Value.Equals(text);
        }
        // if the player has not guessed correctly this will return false and adds letters
        public bool MakeGuess(String guess)
        {
            if (!IsAWord(guess))
            {
                return false;
            }
            bool isGuessCorrect = false;
            // 1. test if letter is in the allGuessedLetters map first
            // 2. test if correct word contains the letter
            // 3. add letter to allGuessedLetters map
            if(guess.Length == 1)
            {
                var guessedLetter = guess[0];
                if (!allGuessedLetters.Contains(guessedLetter))
                {
                    allGuessedLetters.Add(guessedLetter);
                    GuessCount++;
                    if (SecretWord.Contains(guessedLetter))
                    {
                        isGuessCorrect = true;
                    }
                    else
                    {
                        incorrectLetterGuesses.Append(','+guessedLetter);
                    }
                }   
            }
            // test for word guess
            else if(IsWordGuessCorrect(guess))
            {
                isGuessCorrect = IsWordGuessCorrect(guess);
            }
            else
            {
                GuessCount++;
            }
            return isGuessCorrect;
        }

        public bool IsAllLettersGuessedCorrect()
        {
            return SecretWord.ToCharArray().All(e => allGuessedLetters.Contains(e));
        }

        public bool IsWordGuessCorrect(String guess)
        {
            return SecretWord.Equals(guess, StringComparison.CurrentCultureIgnoreCase);
        }

        public void InitializeGame()
        {
            var rng = new Random();
            SecretWord = words[rng.Next(0 , words.Length)];
            correctlyGuessedLetters = new char[SecretWord.Length];
            incorrectLetterGuesses.Clear();
            allGuessedLetters.Clear();
            GuessCount = 0;
            CurrentGuess = "";
        }
        public List<char> GetAllGuessedLetters()
        {
            return allGuessedLetters.ToList();
        }
    }
}