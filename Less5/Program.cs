using System;
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
            Console.WriteLine("Write the text");

            var input = Console.ReadLine();
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

                        int maxDigit = wordsBySentences.SelectMany(s => s)
                            .Max(word => word.Count(c => char.IsDigit(c)));

                        Console.WriteLine("Word(s): \n");

                        foreach (string[] sentence in wordsBySentences)
                             foreach (var word in sentence)
                                 if (word.Count(c => char.IsDigit(c)) == maxDigit)
                                     Console.WriteLine(word);

                        Console.WriteLine($"Maximum number of digits in word is {maxDigit}");

                        break;

                    case "2":

                        int maxLength = wordsBySentences.SelectMany(s => s)
                            .Max(word => word.Length);

                        int countMaxLengthWord = wordsBySentences.SelectMany(s => s)
                            .Count(word => word.Length == maxLength);

                        Console.WriteLine("Word(s): \n");

                        foreach (var sentence in wordsBySentences)
                             foreach (var word in sentence)
                                 if (maxLength == word.Length)
                                        Console.WriteLine(word);

                        Console.WriteLine($"Count most length word is {countMaxLengthWord}\n" +
                                          $"Max length is {maxLength}\n");

                        break;

                    case "3":
                        input = string.Join("", input.Select(x => char.IsDigit(x) ? numbers[x - '0'] : x.ToString()));
                        Console.WriteLine(input);

                        break;
                    case "4":
                        var questSentences = sentences.Where(x => x.EndsWith('?')).ToArray();
                        var exclamationSentences = sentences.Where(x => x.EndsWith('!')).ToArray();

                        foreach (var sentence in questSentences)
                            Console.WriteLine(sentence);

                        foreach (var sentence in exclamationSentences)
                            Console.WriteLine(sentence);

                        break;

                    case "5":

                        var sentensesWithoutComma = sentences.Where(x => !x.Contains(',')).ToArray();

                        foreach (var sentence in sentensesWithoutComma)
                            Console.WriteLine(sentence);

                        break;
                    case "6":
                        string[] words = wordsBySentences.SelectMany(s => s)
                            .Where(word => word.ToLower()[0] == word.ToLower()[word.Length - 1]).ToArray();

                        foreach (var word in words)
                        {
                            Console.WriteLine(word);
                        }

                        break;

                    default:
                        Console.WriteLine("Write only 1-6");
                        break;
                }
            }
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

        private static string[] SentenceToWords(string sentence)
        {
            return sentence.Split(wordPunctuationMarks, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
