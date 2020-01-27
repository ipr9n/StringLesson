using System;
using System.IO;
using System.Linq;


namespace Less5
{
    class Program
    {
        private static readonly string[] sentencePunctuationMarks = { ".", "!", "?" };
        private static readonly string[] wordPunctuationMarks = { ",", ":", "-", ";", " " };
        private static readonly string[] numbers = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };



        static void Main(string[] args)
        {
            string input = "";

            if (args.Length == 0)
            {
                Console.WriteLine("Write the text");
                input = Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Read from: {Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\input.txt");

                if(File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\input.txt"))
                    input = File.ReadAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\input.txt");
                else
                {
                    Console.WriteLine("File doesn't exist, write the text");
                    input = Console.ReadLine();
                }
            }

            string[] sentences = SplitInSentences(input);

            string[][] wordsBySentences = sentences
                .Select(sentence => SentenceToWords(sentence))
                .ToArray();

            while (true)
            {
                Console.WriteLine("----------------------------------------------------------------------------------");

                if (input.Length < 1)
                {
                    Console.WriteLine("Error. Please write the text\n" +
                                      "Press any key to write\n");
                    Console.ReadKey();
                    Console.WriteLine("Write the text");
                    input = Console.ReadLine(); 
                    sentences = SplitInSentences(input);

                    wordsBySentences = sentences
                        .Select(sentence => SentenceToWords(sentence))
                        .ToArray();
                    continue;
                }

                Console.WriteLine("Write 1 to Find words containing the maximum number of digits.\n" +
                                  "Write 2 to Find the longest words and print them\n" +
                                  "Write 3 to Replace the numbers from 0 to 9 with the words “zero”, “one”, ..., “nine\"\n" +
                                  "Write 4 to Display first interrogative and then exclamatory sentences\n" +
                                  "Write 5 to Only display sentences that do not contain commas\n" +
                                  "Write 6 to Find words beginning and ending with the same letter.");

                switch (Console.ReadLine())
                {
                    case "1":
                        string wordsWithContainingMaximumDigits = FindWordsContainingMaximumDigits(wordsBySentences, out int maxDigit);

                        Console.WriteLine($"Word(s): \n{wordsWithContainingMaximumDigits}");
                        Console.WriteLine($"Maximum number of digits in word is {maxDigit}");

                        break;

                    case "2":
                        string longestWords = FindLongestWords(wordsBySentences, out int maxLength, out int countMaxLengthWord);

                        Console.WriteLine($"Count most length word is {countMaxLengthWord}\n" +
                                          $"Max length is {maxLength}\n" +
                                          $"Words: \n {longestWords}");

                        break;

                    case "3":
                        input = string.Join("", input.Select(x => char.IsDigit(x) ? numbers[x - '0'] : x.ToString()));
                        Console.WriteLine(input);

                        break;
                    case "4":

                        string[] questSentences = FindQuestSentences(in sentences);
                        string[] exclamationSentences = FindExclamationSentences(in sentences);

                        foreach (var sentence in questSentences)
                            Console.WriteLine(sentence);

                        foreach (var sentence in exclamationSentences)
                            Console.WriteLine(sentence);

                        break;
                    case "5":

                        var sentensesWithoutComma = FindSentencesWithoutComma(in sentences);

                        foreach (var sentence in sentensesWithoutComma)
                            Console.WriteLine(sentence);

                        break;
                    case "6":
                        string[] words = wordsBySentences.SelectMany(s => s)
                            .Where(word => word.ToLower()[0] == word.ToLower()[word.Length - 1]).ToArray();

                        foreach (var word in words)
                            Console.WriteLine(word);

                        break;

                    default:
                        Console.WriteLine("Write only 1-6");
                        break;
                }
            }
        }

        private static string[] FindQuestSentences(in string[] inputSentences) => inputSentences.Where(x => x.EndsWith('?')).ToArray(); 

        private static string[] FindExclamationSentences(in string[] inputSentences) => inputSentences.Where(x => x.EndsWith('!')).ToArray();

        private static string[] FindSentencesWithoutComma(in string[] inputSentences) => inputSentences.Where(x => !x.Contains(',')).ToArray();

        private static string[] SentenceToWords(string sentence) => sentence.Split(wordPunctuationMarks, StringSplitOptions.RemoveEmptyEntries);

        private static string FindLongestWords(string[][] text, out int maxLength, out int countMaxLengthWord)
        {
            string words = "";

            maxLength = text.SelectMany(s => s)
                .Max(word => word.Length);

            int tempMaxLength = maxLength;

            countMaxLengthWord = text.SelectMany(s => s)
                .Count(word => word.Length == tempMaxLength);

            Console.WriteLine("Word(s): \n");

            foreach (var sentence in text)
            foreach (var word in sentence)
                if (maxLength == word.Length)
                    words += $"{word}\n";

            return words;
        }

        private static string FindWordsContainingMaximumDigits(string[][] text, out int maxDigit)
        {
           maxDigit = text.SelectMany(s => s)
                .Max(word => word.Count(c => char.IsDigit(c)));

           string words = "";

            foreach (string[] sentence in text)
            foreach (var word in sentence)
                if (word.Count(c => char.IsDigit(c)) == maxDigit)
                    words += $"{word}\n";

            return words;

        }

        private static string[] SplitInSentences(string text)
        {
            string tempText = text;

            foreach (var mark in sentencePunctuationMarks)
                tempText = tempText.Replace(mark, $"{mark}#");

            while (tempText.Contains("#.") || tempText.Contains("#?") || tempText.Contains("#!"))
            {
                tempText = tempText
                        .Replace("#.", ".#")
                        .Replace("#?", "?#")
                        .Replace("#!", "!#");
            }

            return tempText
                .Split('#')
                .Where(sentence => sentence.Length != 0)
                .ToArray();

        }
    }
}
