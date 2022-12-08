namespace typing;

class Program
{
    static void Main(string[] args)
    {
        bool ghost = true;

        TypingTest newtest = new("the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. the quick brown fox jumped over the lazy dog. ", 30, ghost);
    
        Console.Write("\nPress any key to continue");
        Console.ReadKey();
    }
}
