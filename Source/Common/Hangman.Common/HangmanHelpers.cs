using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hangman.Common
{
    public static class HangmanHelpers
    {
        public static string[] GetGuessWord(string dbWord)
        {
            var wordArr = new string[dbWord.Length];
            for (int i = 1; i < wordArr.Length - 1; i++)
            {
                wordArr[i] = "_";
            }
            wordArr[0] = dbWord[0].ToString();
            wordArr[dbWord.Length - 1] = dbWord[dbWord.Length - 1].ToString();

            return wordArr;
        }

        public static IList<int> GetIndexesOfGuessing(string word, string letter)
        {
            var indexList = Regex.Matches(word, letter).Cast<Match>()
                    .Select(m => m.Index)
                    .ToList();

            return indexList;
        }

        public static string[] ConstructUpdatedWord(string[] wordArr, IList<int> indexes, string letter)
        {
            if (indexes.Count == 0
                || indexes[0] == 0
                || indexes[0] == wordArr.Length - 1
                || indexes[indexes.Count - 1] == wordArr.Length - 1)
            {
                return wordArr;
            }
            else
            {
                string[] result = new string[wordArr.Length];

                for (int j = 0; j < wordArr.Length; j++)
                {
                    result[j] = wordArr[j];
                }

                for (int i = 0; i < indexes.Count; i++)
                {
                    result[indexes[i]] = letter;
                }
                return result;
            }

        }
    }
}
