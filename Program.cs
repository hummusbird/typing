namespace typing;

class Program
{
    static void Main(string[] args)
    {
        bool random = false;
        int length = 0;
        string prompt = "the quick brown fox jumps over the lazy dog";
        int timer = 0;
        bool ghost = false;
        bool autocorrect = false;
        bool incognito = false;

        foreach (string arg in args)
        {
            int pos = Array.FindIndex(args, v => v == arg);

            switch (arg)
            {
                case "--random":
                case "-r":
                    random = true;
                    break;

                case "--timer": // timer length
                case "-t":
                    if (args.Length >= pos + 2) { timer = int.Parse(args[pos + 1]); }
                    break;

                case "--length": // random wordgen length
                case "-l":
                    if (args.Length >= pos + 2) { length = int.Parse(args[pos + 1]); }
                    break;

                case "--incognito": // completely disable saving of scores
                case "-i":
                    incognito = !incognito;
                    break;

                case "--ghost": // whether prompt should be above or behind user input
                case "-g":
                    ghost = !ghost;
                    break;

                case "--autocorrect": // whether incorrect keystrokes should be shown correctly or not
                case "-a":
                    autocorrect = !autocorrect;
                    break;

                case "--prompt":
                case "-p":
                    if (args.Length >= pos + 2) { prompt = PromptGen.ExtractPromptFromArgs(args); }
                    break;

                case "--file": // load prompt from file
                case "-f":
                    if (args.Length >= pos + 2) { prompt = PromptGen.ExtractPromptFromFile(args[pos + 1]); }
                    break;
            }
        }

        // ensures correct length even if length is defined after prompt
        if (random) { prompt = PromptGen.GenerateRandomPromptFromWordlist(length); }
        else { prompt = PromptGen.TrimPromptToLength(prompt, length); }

        // TODO: interactive settings menu
        // TODO: default settings .conf file

        TypingTest test = new(prompt, timer, ghost, autocorrect);

        if (String.IsNullOrEmpty(prompt)) { Console.WriteLine("Prompt cannot be empty."); }
        else
        {
            test.RunTest();

            Results.PrintStats(test);

            if (!incognito) { Results.SaveStats(test); }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\nincognito enabled - not saving results for this session!");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        Thread.Sleep(500);
        Console.Write("\nPress any key to continue");

        Console.ReadKey();
    }
}
