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

        public static string ExtractPromptFromArgs(string[] args, int length)
        {
            int pos = Array.FindIndex(args, v => (v == "-p") || (v == "--prompt"));

            if (!(args.Length >= pos + 2)) { return null!; } // if no prompt, return null

            string prompt = args[pos + 1];

            return TrimPromptToLength(prompt, length);
        }

        public static string TrimPromptToLength(string in_prompt, int length)
        {
            string[] words = in_prompt.Split(" ");

            string prompt = "";

            if (length <= 0) { return in_prompt; }

            for (int i = 0; (i < words.Length) && (i < length); i++)
            {
                prompt += words[i] + " ";
            }

            return prompt.Trim();
        }
    }
}