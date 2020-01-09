using System;
using System.Linq;


namespace Less5
{
    class Program
    {
        public static char[] sentencePunctuationMarks = new char[] { '.', '!', '?' };
        public static char[] wordPunctuationMarks = new char[] { ',', ':', '-', ';', ' ' };
        private static int maxDigit = 0;
        private static int maxLength = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Write the text");
            var input = Console.ReadLine();
            var sentences = SplitInSentences(input);

            Console.WriteLine("Write 1 to Find words containing the maximum number of digits.\n" +
                              "Write 2 to Find the longest words and print them\n" +
                              "Write 3 to Replace the numbers from 0 to 9 with the words “zero”, “one”, ..., “nine\"");

            switch (Console.ReadLine())
            {
                case "1":
                    string[] tempWords;

                    for (int i = 0; i < sentences.Length; i++)
                    {
                        tempWords = SentenceToWords(sentences[i]);

                        for (int j = 0; j < tempWords.Length; j++)
                        {

                            char[] tempCharArray = tempWords[j].ToCharArray();
                            var count = tempCharArray.Where((n) => n >= '0' && n <= '9').Count();
                            if (count > maxDigit) maxDigit = count;
                        }

                    }

                    for (int i = 0; i < sentences.Length; i++)
                    {
                        tempWords = SentenceToWords(sentences[i]);

                        for (int j = 0; j < tempWords.Length; j++)
                        {

                            char[] tempCharArray = tempWords[j].ToCharArray();
                            var count = tempCharArray.Where((n) => n >= '0' && n <= '9').Count();
                            if (count == maxDigit) Console.WriteLine(tempWords[j]);
                        }
                    }

                    Console.WriteLine(maxDigit);

                    break;

                case "2":
                    for (int i = 0; i < sentences.Length; i++)
                    {
                        tempWords = SentenceToWords(sentences[i]);

                        for (int j = 0; j < tempWords.Length; j++)
                            if (maxLength < tempWords[j].Length)
                                maxLength = tempWords[j].Length;
                    }

                    for (int i = 0; i < sentences.Length; i++)
                    {
                        tempWords = SentenceToWords(sentences[i]);

                        for (int j = 0; j < tempWords.Length; j++)
                            if (maxLength == tempWords[j].Length)
                                Console.WriteLine(tempWords[j]);
                    }

                    break;

                case "3":
                    var numbers = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

                    input = string.Join("", input.Select(x => char.IsDigit(x) ? numbers[x - '0'] : x.ToString()));
                    Console.WriteLine(input);

                    break;
            }

            //for (int i = 0; i < sentences.Length; i++)
            //    Console.WriteLine(sentences[i]);

            //            var words = SentenceToWords(sentences[Int32.Parse(Console.ReadLine())]);

            //for(int i = 0; i<words.Length; i++)
            //    Console.WriteLine(words[i]);
            Console.ReadKey();
        }

        public static string[] SplitInSentences(string text)
        {
            string tempText = text;

            for (int i = 0; i < sentencePunctuationMarks.Length; i++)
                tempText = tempText.Replace(sentencePunctuationMarks[i].ToString(), $"{sentencePunctuationMarks[i]}#");

            while (tempText.Contains("#.") || tempText.Contains("#?") || tempText.Contains("#!"))
            {
                while (tempText.Contains("#."))
                {
                    tempText = tempText.Replace("#.", ".#");
                }

                while (tempText.Contains("#?"))
                {
                    tempText = tempText.Replace("#?", "?#");
                }

                while (tempText.Contains("#!"))
                {
                    tempText = tempText.Replace("#!", "!#");
                }
            }
            var sentences = tempText.Split('#');

            for (int i = 0; i < sentences.Length; i++)
                sentences = sentences.Where(sentence => sentence.Length != 0).ToArray();

            return sentences;
        }

        public static string[] SentenceToWords(string text)
        {
            var words = text.Split(wordPunctuationMarks);
            words = words.Where(word => word.Length != 0).ToArray();
            return words;
        }
    }
}
