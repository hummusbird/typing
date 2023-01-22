using System;
using System.Collections.Generic;
using System.Text;

namespace typing
{
    static class PromptGen
    {
        public static string GenerateRandomPromptFromWordlist(int length, string wordlist = "wordlist_10000.txt")
        {
            var rand = new Random();

            string[] words = System.IO.File.ReadAllLines(wordlist);
            string output = "";

            for (int i = 0; i < length; i++)
            {
                output += words[rand.Next(10000)] + " ";
            }

            return output.Trim();
        }
    }
}