using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HangmanGame
{
    public class HangmanGameLogic
    {
        private HashSet<char> allGuessedLetters;
        public const int MaxGuesses = 10;
        private String currentGuess;
        private String[] words = new String[]{"person","jobb","spel","grafik","text","orm","cyckelpump","bössa","paraply","citron","päron","apelsin","tacos","pizza","läsk","studier","jul","ekvation",
"namn","rep","godis","chips","bil","spårvagn","sjukhus","kossa"};
        public StringBuilder IncorrectLetterGuesses {get; private set;} 
        public String SecretWord {get; set;}
        public int GuessCount {get; private set;}
        public bool IsGameOver => GuessCount >= MaxGuesses;
        public Char[] RevealedLetters { get; set; }

        public HangmanGameLogic()
        {
            IncorrectLetterGuesses = new StringBuilder();
            allGuessedLetters = new HashSet<char>(29);
        }

        public void InitializeGame()
        {
            var rng = new Random();
            SecretWord = words[rng.Next(0 , words.Length)];
            UpdateRevealedLetters();
            IncorrectLetterGuesses.Clear();
            allGuessedLetters.Clear();
            GuessCount = 0;
            currentGuess = "";
        }

        public void UpdateRevealedLetters()
        {
            bool isWordGuessCorrect = IsWordGuessCorrect(currentGuess);
            RevealedLetters = new char[SecretWord.Length];
            for (int i = 0; i < SecretWord.Length; i++)
            {
                char letter = SecretWord[i];
                bool hasSeenLetter = allGuessedLetters.Contains(letter) || isWordGuessCorrect;
                RevealedLetters[i] = hasSeenLetter ? letter : '_';
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

            currentGuess = guess;
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
                    if (IsAllLettersGuessedCorrect())
                    {
                        return true;
                    }
                    GuessCount++;
                    if (!SecretWord.Contains(guessedLetter))
                    {
                        IncorrectLetterGuesses.Append(','+guessedLetter.ToString());
                    }
                }   
            }
            // test for word guess
            if( guess.Length == SecretWord.Length && IsWordGuessCorrect(guess))
            {
                isGuessCorrect = IsWordGuessCorrect(guess);
            }
            else if(guess.Length == SecretWord.Length)
            {
                GuessCount++;
            }

            UpdateRevealedLetters();
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


        public List<char> GetAllGuessedLetters()
        {
            return allGuessedLetters.ToList();
        }
    }
}