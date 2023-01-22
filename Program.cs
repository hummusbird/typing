namespace typing;

class Program
{
    static void Main(string[] args)
    {
        // TODO: handle args
        // TODO: interactive settings menu
        // TODO: default settings .conf file
        // TODO: prompt gen

        string prompt = PromptGen.GenerateRandomPromptFromWordlist(10); // "the quick brown fox jumped over the lazy dog, before going on a shopping trip to Tesco's. The dog bought some cheese.";
        int timer = 25;
        bool ghost = true;
        bool autocorrect = false;

        TypingTest newtest = new(@prompt, timer, ghost, autocorrect);

        newtest.RunTest();

        Console.Write("\nPress any key to continue");
        Console.ReadKey();
    }
}
