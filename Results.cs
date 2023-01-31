using Newtonsoft.Json;

namespace typing
{
    static class Results
    {
        public static void PrintStats(TypingTest test)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"TIME:       {test.TimeTaken} seconds");
            Console.WriteLine($"LENGTH:     {test.CountThroughPrompt} characters");
            Console.WriteLine($"WPM:        {(int)(((test.KeystrokeCount - test.Misinputs) / 5) / (test.TimeTaken / 60))}"); // WPM = (Keystrokes / 5) / Time taken (minutes)
            Console.WriteLine($"RAW WPM:    {(int)((test.KeystrokeCount / 5) / (test.TimeTaken / 60))}"); // passing to (int) removes decimal places
            Console.WriteLine($"ACCURACY:   {(double)((int)((test.KeystrokeCount - test.Misinputs) / test.KeystrokeCount * 10000)) / 100}%"); // 2 decimal places
            Console.WriteLine($"MISINPUTS:  {test.Misinputs}/{test.KeystrokeCount}");
        }

        public static void SaveStats(TypingTest test)
        {
            Console.WriteLine("\nSaving results...");

            string filename = DateTime.Now.ToString().Replace("/", "-") + ".json";

            if (!Directory.Exists("typing-results")) { Directory.CreateDirectory("typing-results"); } // create results folder if one doesn't exist

            using (StreamWriter SW = new StreamWriter("typing-results/" + filename)) // save the test under the current date & time
            {
                SW.WriteLine(JsonConvert.SerializeObject(test, Formatting.Indented));
                Console.WriteLine($"{filename} saved!");
            }
        }

        public static void CompareAgainstPB(TypingTest test)
        {
            // TODO: iterate through all files & find best scores
            // TODO: use filename to compare
        }

        public static void PrintCoolInformationMessage()
        {
            // improvement since first test
            // highest / lowest results 
            // congrats on first test
            // streak counter?
            // 10, 25, 50, 100, 1000 test milestones
            // did you know
            // delete folder to restart
            // use -i to prevent scores being saved
            // facts idk

            // "you cheated" message
        }
    }
}