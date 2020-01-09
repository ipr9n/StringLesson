using System;
using System.Linq;


namespace Less5
{
    class Program
    {
        private static readonly char[] sentencePunctuationMarks = { '.', '!', '?' };
        private static readonly char[] wordPunctuationMarks = { ',', ':', '-', ';', ' ' };
        private static int maxDigit = 0;
        private static int maxLength = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Write the text");
            var input = Console.ReadLine();
            string[] sentences = SplitInSentences(input);

            Console.WriteLine("Write 1 to Find words containing the maximum number of digits.\n" +
                              "Write 2 to Find the longest words and print them\n" +
                              "Write 3 to Replace the numbers from 0 to 9 with the words “zero”, “one”, ..., “nine\"\n" +
                              "Write 4 to Display first interrogative and then exclamatory sentences\n" +
                              "Write 5 to Only display sentences that do not contain commas");

            switch (Console.ReadLine())
            {
                case "1":
                    string[] tempWords;

                    foreach (var sentence in sentences)
                    {
                        tempWords = SentenceToWords(sentence);

                        foreach (var word in tempWords)
                        {
                            char[] tempCharArray = word.ToCharArray();
                            var count = tempCharArray.Count(n => n >= '0' && n <= '9');

                            if (count > maxDigit) maxDigit = count;
                        }
                    }

                    foreach (var sentence in sentences)
                    {
                        tempWords = SentenceToWords(sentence);

                        foreach (var word in tempWords)
                        {
                            char[] tempCharArray = word.ToCharArray();
                            var count = tempCharArray.Count(n => n >= '0' && n <= '9');
                            if (count == maxDigit) Console.WriteLine(word);
                        }
                    }

                    Console.WriteLine(maxDigit);

                    break;

                case "2":
                    foreach (var sentence in sentences)
                    {
                        tempWords = SentenceToWords(sentence);

                        foreach (var word in tempWords)
                            if (maxLength < word.Length)
                                maxLength = word.Length;
                    }

                    foreach (var sentence in sentences)
                    {
                        tempWords = SentenceToWords(sentence);

                        foreach (var word in tempWords)
                            if (maxLength == word.Length)
                                Console.WriteLine(word);
                    }

                    break;

                case "3":
                    var numbers = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

                    input = string.Join("", input.Select(x => char.IsDigit(x) ? numbers[x - '0'] : x.ToString()));
                    Console.WriteLine(input);

                    break;
                case "4":
                    var questSentences = sentences.Where(x => x.EndsWith('?')).ToArray();
                    var exclamationSentances = sentences.Where(x => x.EndsWith('!')).ToArray();

                    foreach (var sentence in questSentences)
                        Console.WriteLine(sentence);

                    foreach (var sentence in exclamationSentances)
                        Console.WriteLine(sentence);

                    break;

                case "5":

                    var sentensesWithoutComma = sentences.Where(x => !x.Contains(',')).ToArray();

                    foreach (var sentence in sentensesWithoutComma)
                        Console.WriteLine(sentence);

                    break;
            }

            //for (int i = 0; i < sentences.Length; i++)
            //    Console.WriteLine(sentences[i]);

            //            var words = SentenceToWords(sentences[Int32.Parse(Console.ReadLine())]);

            //for(int i = 0; i<words.Length; i++)
            //    Console.WriteLine(words[i]);
            Console.ReadKey();
        }

        private static string[] SplitInSentences(string text)
        {
            string tempText = text;

            foreach (var mark in sentencePunctuationMarks)
                tempText = tempText.Replace(mark.ToString(), $"{mark}#");

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

        private static string[] SentenceToWords(string text)
        {
            return text
                .Split(wordPunctuationMarks)
                .Where(word => word.Length != 0)
                .ToArray();
        }
    }
}
