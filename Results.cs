namespace typing
{
    static class Results
    {
        // TODO: decide on order to run these, and whether they should call eachother or all be called from Program.cs

        public static void PrintStats(TypingTest test)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            double TimeTaken = test.GetTimeTaken();
            double KeystrokeCount = test.GetKeystrokeCount();
            int CountThroughPrompt = test.GetCountThroughPrompt();
            double Misinputs = test.GetMisinputs();

            Console.WriteLine($"TIME:       {TimeTaken} seconds");
            Console.WriteLine($"LENGTH:     {CountThroughPrompt} characters");
            Console.WriteLine($"WPM:        {(int)(((KeystrokeCount - Misinputs) / 5) / (TimeTaken / 60))}"); // WPM = (Keystrokes / 5) / Time taken (minutes)
            Console.WriteLine($"RAW WPM:    {(int)((KeystrokeCount / 5) / (TimeTaken / 60))}"); // passing to (int) removes decimal places
            Console.WriteLine($"ACCURACY:   {(double)((int)((KeystrokeCount - Misinputs) / KeystrokeCount * 10000)) / 100}%"); // 2 decimal places
            Console.WriteLine($"MISINPUTS:  {Misinputs}/{KeystrokeCount}");
        }

        public static void SaveStats(TypingTest test)
        {

        }

        public static void CompareAgainstPB(TypingTest test)
        {

        }
    }
}