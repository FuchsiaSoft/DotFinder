using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DotFinder
{
    class Program
    {
        private static int[] _acceptedPattern = new int[12]
        {
            1,1,3,1,3,1,1,1,3,1,1,3
        };

        static void Main(string[] args)
        {
            string filePath = @"C:\Users\Chris\Downloads\WikiFiles\enwiki-20161201-pages-articles\enwiki-20161201-pages-articles.xml";
            int[] currentPattern = new int[12];

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                //continue reading as long as we have wiki articles to read
                while (reader.ReadToFollowing("title"))
                {
                    string title = reader.ReadElementContentAsString();
                    reader.ReadToFollowing("text");
                    string articleBody = reader.ReadElementContentAsString();
                    articleBody = Regex.Replace(articleBody, "[^(a-zA-Z \\s)]", "");
                    string[] words = articleBody.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //pattern we want in syllabes is 1,3,1 - 1,3,1 - 1,3,1 - 1,3,1
                    //over 12 words in sequence
                    if (words.Length <= 12) continue;
                    int currentIndex = 0;
                    List<string> currentWords = new List<string>();

                    while (currentIndex < words.Length - 12)
                    {
                        currentWords.Clear();

                        for (int i = currentIndex; i < currentIndex + 12; i++)
                        {
                            currentWords.Add(words[i]);
                        }

                        for (int i = 0; i < 12; i++)
                        {
                            currentPattern[i] = SyllableCount(currentWords[i]);
                        }

                        
                        if (currentPattern.SequenceEqual(_acceptedPattern))
                        {
                            Console.ResetColor();
                            Console.Write("[ ");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            foreach (int item in currentPattern)
                            {
                                Console.Write(item + " ");
                            }
                            Console.ResetColor();
                            Console.Write(" ]    ");

                            for (int i = 0; i < 12; i++)
                            {
                                Console.Write(currentWords[i].Replace("\n", "").Replace("\r", "") + " ");
                            }
                            Console.WriteLine();
                            Console.ReadLine();

                        }

                        if (currentPattern.SequenceEqual(_acceptedPattern))
                        {
                            
                        }

                        currentIndex++;
                    }

                    Console.ResetColor();
                    Console.Write("[ ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("PAGE FINISHED");
                    Console.ResetColor();
                    Console.Write(" ]    ");
                    Console.WriteLine(title);

                }




            }



        }

        //Nabbed from:
        //http://codereview.stackexchange.com/questions/9972/syllable-counting-function
        private static int SyllableCount(string word)
        {
            word = word.ToLower().Trim();
            bool lastWasVowel = false;
            var vowels = new[] { 'a', 'e', 'i', 'o', 'u', 'y' };
            int count = 0;

            //a string is an IEnumerable<char>; convenient.
            foreach (var c in word)
            {
                if (vowels.Contains(c))
                {
                    if (!lastWasVowel)
                        count++;
                    lastWasVowel = true;
                }
                else
                    lastWasVowel = false;
            }

            if ((word.EndsWith("e") || (word.EndsWith("es") || word.EndsWith("ed")))
                  && !word.EndsWith("le"))
                count--;

            return count;
        }
    }
}
