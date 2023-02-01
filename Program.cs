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

        bool newTest = true; // set to false to retry with the same settings

        foreach (string arg in args)
        {
            int pos = Array.FindIndex(args, v => v == arg);

            switch (arg)
            {
                case "--random": // generate random words from wordlist_10000.txt
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

                case "--prompt": // load prompt from arg
                case "-p":
                    if (args.Length >= pos + 2) { prompt = PromptGen.ExtractPromptFromArgs(args); }
                    break;

                case "--file": // load prompt from file
                case "-f":
                    if (args.Length >= pos + 2) { prompt = PromptGen.ExtractPromptFromFile(args[pos + 1]); }
                    break;
            }
        }

        while (true)
        {
            if (newTest)
            {
                // ensures correct length even if length is defined after prompt
                // this is rerun if newTest is true, to generate a new random prompt
                if (random) { prompt = PromptGen.GenerateRandomPromptFromWordlist(length); }
                else { prompt = PromptGen.TrimPromptToLength(prompt, length); }

                newTest = false;
            }

            // TODO: interactive settings menu
            // TODO: default settings .conf file

            TypingTest test = new(prompt, timer, ghost, autocorrect);

            if (String.IsNullOrEmpty(prompt)) { Console.WriteLine("Prompt cannot be empty."); Environment.Exit(1); }

            test.RunTest();

            Results.PrintStats(test);

            Results.CompareAgainstTests(test);

            if (!incognito) // do not save results if -i arg set
            {
                Results.SaveStats(test);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nincognito enabled - no results saved");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("\n[any] Exit - [R] Retry - [N] New test");
            Console.Write("> ");

            string input = Console.ReadLine()!.ToLower();

            switch (input)
            {
                case "new test":
                case "new":
                case "n":
                    newTest = true;
                    break;

                case "retry":
                case "r":
                    Console.WriteLine("Retrying test...");
                    break;

                default:
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
