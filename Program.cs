namespace typing;

class Program
{
    static void Main(string[] args)
    {
        // TODO: handle args
        // TODO: interactive settings menu
        // TODO: default settings .conf file

        string prompt = "the quick brown fox jumped over the lazy dog, before going on a shopping trip to Tesco's. The dog bought some cheese.";
        int timer = 100;
        bool ghost = false;
        bool autocorrect = false;

        TypingTest newtest = new(@prompt, timer, ghost, autocorrect);

        newtest.RunTest();

        Console.Write("\nPress any key to continue");
        Console.ReadKey();
    }
}
