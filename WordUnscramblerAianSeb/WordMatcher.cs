using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordUnscramblerAianSeb
{
    class WordMatcher
    {
        public List<MatchedWord> Match(string[] scrambledWords, string[] wordList)
        {
            List<MatchedWord> matchedWords = new List<MatchedWord>();

            foreach (var scrambledWord in scrambledWords)
            {
                foreach (var word in wordList)
                {
                    //scrambled word matches with word
                    if (scrambledWord.Equals(word, StringComparison.OrdinalIgnoreCase))
                    {
                        matchedWords.Add(BuildMatchedWord(scrambledWord, word));
                    }
                    //scrambled word doesn't match with word
                    else
                    {
                        var arrayScrambledWord = scrambledWord.ToCharArray();
                        var arrayWord = word.ToCharArray();

                        Array.Sort(arrayScrambledWord);
                        Array.Sort(arrayWord);

                        var sortedScrambledWord = new string(arrayScrambledWord);
                        var sortedWord = new string(arrayWord);

                        if (sortedScrambledWord.Equals(sortedWord))
                        {
                            matchedWords.Add(BuildMatchedWord(scrambledWord, word));
                        }
                    }
                }
            }

            // Implement code here.
            // Work with "scrambledWords" and "matchedWords".

            MatchedWord BuildMatchedWord(string scrambledWord, string word)
            {
                // Build a matched-word object here, so that you can return it.
                MatchedWord matchedWord = new MatchedWord
                {
                    ScrambledWord = scrambledWord,
                    Word = word
                };
                return matchedWord;
            }
            return matchedWords;
        }

    }
}
