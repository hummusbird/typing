namespace typing;

class Program
{
    static void Main(string[] args)
    {
        // TODO: main menu
        // TODO: handle args

        bool ghost = true;
        string prompt = "the quick brown fox jumped over the lazy dog, before going on a shopping trip to Tesco's. The dog bought some cheese.";
        int timer = 5;

        TypingTest newtest = new(@prompt, timer, ghost);
    
        newtest.RunTest();

        Console.Write("\nPress any key to continue");
        Console.ReadKey();
    }
}
