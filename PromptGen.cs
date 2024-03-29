namespace typing
{
    static class PromptGen
    {
        public static string GenerateRandomPromptFromWordlist(int length, string wordlist)
        {
            var rand = new Random();

            string[] words = System.IO.File.ReadAllLines(wordlist);
            string output = "";

            length = length > words.Length ? words.Length : length; // ensure length is less than words max

            Console.WriteLine(length);
            Console.WriteLine(words.Length);

            for (int i = 0; i < length; i++)
            {
                output += words[rand.Next(words.Length)] + " ";
            }

            return output.Trim();
        }

        public static string ExtractPromptFromArgs(string[] args)
        {
            int pos = Array.FindIndex(args, v => (v == "-p") || (v == "--prompt"));

            if (!(args.Length >= pos + 2)) { return null!; } // if no prompt, return null

            return args[pos + 1];
        }

        public static string ExtractPromptFromFile(string filename) // TODO
        {
            return filename;
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