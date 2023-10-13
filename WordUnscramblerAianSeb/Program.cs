using System;
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
            Console.WriteLine("Choose your language preference:");
            Console.WriteLine("1 - English");
            Console.WriteLine("2 - French (French Canada)");

            string choice = Console.ReadLine();

            if (choice == "2")
            {
                // Set the program to use French (French Canada) culture.
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-CA");
                _resourceManager = new ResourceManager("WordUnscramblerAianSeb.Properties.stringFR", typeof(Program).Assembly);
            }
            else
            {
                // Default to English culture.
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                _resourceManager = new ResourceManager("WordUnscramblerAianSeb.Properties.stringEN", typeof(Program).Assembly);
            }

            // Access localized strings from resource files.
            string welcomeMessage = _resourceManager.GetString("WelcomeMessage");

            Console.WriteLine(welcomeMessage);

            try
            {
                Console.WriteLine("Enter scrambled word(s) manually or as a file: F - file / M - manual");

                String option = Console.ReadLine() ?? throw new Exception("String is empty");

                switch (option.ToUpper())
                {
                    case "F":
                        Console.WriteLine("Enter full path including the file name: ");
                        ExecuteScrambledWordsInFileScenario();
                        break;
                    case "M":
                        Console.WriteLine("Enter word(s) manually (separated by commas if multiple): ");
                        ExecuteScrambledWordsManualEntryScenario();
                        break;
                    default:
                        Console.WriteLine("The entered option was not recognized.");
                        break;
                }

                Console.ReadLine();


            }
            catch (Exception ex)
            {
                Console.WriteLine("The program will be terminated." + ex.Message);

            }
        }

        private static void ExecuteScrambledWordsInFileScenario()
        {
            var filename = Console.ReadLine();
            string[] scrambledWords = _fileReader.Read(filename);
            DisplayMatchedUnscrambledWords(scrambledWords);
        }

        private static void ExecuteScrambledWordsManualEntryScenario()
        {
            var nameFile = Console.ReadLine();
            string input = Console.ReadLine();
            string[] scrambledWords = input.Split(',');
            DisplayMatchedUnscrambledWords(scrambledWords);
        }

        private static void DisplayMatchedUnscrambledWords(string[] scrambledWords)
        {
            //read the list of words from the system file. 
            string[] wordList = _fileReader.Read("wordlist.txt");

            //call a word matcher method to get a list of structs of matched words.
            List<MatchedWord> matchedWords = _wordMatcher.Match(scrambledWords, wordList);

            if (matchedWords.Any())
            {
                foreach (var matchedWord in matchedWords)
                {
                    Console.WriteLine($"Match found for {matchedWord.ScrambledWord}: {matchedWord.Word}");
                }
            }
            else
            {
                Console.WriteLine("No matches found.");
            }
        }
    }
}
