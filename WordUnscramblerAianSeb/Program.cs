﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;

namespace WordUnscramblerAianSeb
{
    class Program
    {
        private static readonly FileReader _fileReader = new FileReader();
        private static readonly WordMatcher _wordMatcher = new WordMatcher();
        private static ResourceManager _resourceManager;

        static void Main(string[] args)
        {
            Console.WriteLine(Constants.LanguagePreference);
            Console.WriteLine(Constants.english);
            Console.WriteLine(Constants.french);

            string choice = Console.ReadLine();

            if (choice == "2")
            {
                // Set the program to use French (French Canada) culture.
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-CA");
                _resourceManager = new ResourceManager(Constants.stringFR, typeof(Program).Assembly);
            }
            else
            {
                // Default to English culture.
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                _resourceManager = new ResourceManager(Constants.stringEN, typeof(Program).Assembly);
            }

            // Access localized strings from resource files.
            string welcomeMessage = _resourceManager.GetString("WelcomeMessage");

            Console.WriteLine(welcomeMessage);

            while(true)
            {
                Console.WriteLine(_resourceManager.GetString("Options"));

                String option = Console.ReadLine() ?? throw new Exception(_resourceManager.GetString("emptyString"));

                switch (option.ToUpper())
                {
                    case "F":
                        Console.WriteLine(_resourceManager.GetString("OptionF"));
                        ExecuteScrambledWordsInFileScenario();
                        break;
                    case "M":
                        Console.WriteLine(_resourceManager.GetString("OptionM2"));
                        ExecuteScrambledWordsManualEntryScenario();
                        break;
                    default:
                        Console.WriteLine(_resourceManager.GetString("FalseOption"));
                        break;
                }

                Console.WriteLine(_resourceManager.GetString("Continuation"));

                string continueOption = Console.ReadLine() ?? string.Empty;

                while (!continueOption.Equals("Y", StringComparison.OrdinalIgnoreCase) &&
                   !continueOption.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(_resourceManager.GetString("Redo"));
                    continueOption = Console.ReadLine() ?? string.Empty;
                }

                if (continueOption.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

            }

        }

        private static void ExecuteScrambledWordsInFileScenario()
        {
            var filename = Console.ReadLine();
            string[] scrambledWords = _fileReader.Read(filename);
            if (scrambledWords == null || !scrambledWords.Any())
            {
                Console.WriteLine(_resourceManager.GetString(Constants.FileNotFound));
                return;
            }
            DisplayMatchedUnscrambledWords(scrambledWords);
        }

        private static void ExecuteScrambledWordsManualEntryScenario()
        {
            string input = Console.ReadLine();
            string[] scrambledWords = input.Split(',');
            DisplayMatchedUnscrambledWords(scrambledWords);
        }

        private static void DisplayMatchedUnscrambledWords(string[] scrambledWords)
        {
            //read the list of words from the system file. 
            string[] wordList = _fileReader.Read(Constants.WordList);

            //call a word matcher method to get a list of structs of matched words.
            List<MatchedWord> matchedWords = _wordMatcher.Match(scrambledWords, wordList);

            if (matchedWords.Any())
            {
                Console.WriteLine(_resourceManager.GetString("MatchedWord"));
                foreach (var matchedWord in matchedWords)
                {
                    Console.WriteLine($"{_resourceManager.GetString("IsMatch")} for {matchedWord.ScrambledWord}: {matchedWord.Word}");
                }
            }
            else
            {
                Console.WriteLine(_resourceManager.GetString(Constants.FileNotFound));
            }
        }
    }
}
