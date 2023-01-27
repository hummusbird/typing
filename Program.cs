namespace typing;

class Program
{
    static void Main(string[] args)
    {
        int length = 10;
        string prompt = PromptGen.GenerateRandomPromptFromWordlist(length); // "the quick brown fox jumped over the lazy dog, before going on a shopping trip to Tesco's. The dog bought some cheese.";
        int timer = 10;
        bool ghost = false;
        bool autocorrect = false;

        foreach (string arg in args)
        {
            int pos = Array.FindIndex(args, v => v == arg);

            switch (arg)
            {
                case "--timer": // timer length
                case "-t":
                    if (args.Length >= pos + 2)
                    {
                        timer = int.Parse(args[pos + 1]);
                    }
                    break;

                case "--length": // random wordgen length
                case "-l":
                    if (args.Length >= pos + 2)
                    {
                        timer = int.Parse(args[pos + 1]);
                    }
                    break;

                case "--ghost": // whether prompt should be above or behind user input
                case "-g":
                    ghost = !ghost;
                    break;

                case "--autocorrect": // whether incorrect keystrokes should be shown correctly or not
                case "-a":
                    autocorrect = !autocorrect;
                    break;
            }
        }

        // TODO: interactive settings menu
        // TODO: default settings .conf file

        TypingTest newtest = new(@prompt, timer, ghost, autocorrect);

        newtest.RunTest();

        Thread.Sleep(2000);
        Console.Write("\nPress any key to continue");

        Console.ReadKey();
    }
}
