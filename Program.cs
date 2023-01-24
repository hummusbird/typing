namespace typing;

class Program
{
    static void Main(string[] args)
    {
        string prompt = PromptGen.GenerateRandomPromptFromWordlist(10); // "the quick brown fox jumped over the lazy dog, before going on a shopping trip to Tesco's. The dog bought some cheese.";
        int timer = 10;
        bool ghost = true;
        bool autocorrect = false;

        foreach (string arg in args)
        {
            int pos = Array.FindIndex(args, v => v == arg);

            switch (arg)
            {
                case "-t": // timer length
                    if (args.Length >= pos + 2)
                    {
                        timer = int.Parse(args[pos + 1]);
                    }
                    break;
            }
        }


        // TODO: interactive settings menu
        // TODO: default settings .conf file
        // TODO: prompt gen

        TypingTest newtest = new(@prompt, timer, ghost, autocorrect);

        newtest.RunTest();

        Thread.Sleep(2000);
        Console.Write("\nPress any key to continue");

        Console.ReadKey();
    }
}
