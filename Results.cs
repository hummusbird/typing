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

        public static void CompareAgainstTests(TypingTest test)
        {
            string[] files = Directory.GetFiles("typing-results");

            if (files.Length <= 1) { Console.WriteLine("\nCongrats on completing your first test!"); return; }

            List<TypingTest>? tests = new List<TypingTest>();

            double numoflowerwpmtests = 0;
            double numofloweraccurracytests = 0;

            double HighestWPM = 0;
            double LongestTest = 0;
            double HighestAccuracy = 0;

            foreach (string filename in files)
            {
                if (filename.EndsWith(".json"))
                {
                    using (StreamReader SR = new StreamReader(filename))
                    {
                        try
                        {
                            string json = SR.ReadToEnd();

                            TypingTest jsontest = JsonConvert.DeserializeObject<TypingTest>(json)!;

                            int wpm = (int)(((jsontest.KeystrokeCount - jsontest.Misinputs) / 5) / (jsontest.TimeTaken / 60));
                            double accuracy = (double)((int)((jsontest.KeystrokeCount - jsontest.Misinputs) / jsontest.KeystrokeCount * 10000)) / 100;

                            if (wpm > HighestWPM) { HighestWPM = wpm; }
                            if (jsontest.CountThroughPrompt > LongestTest) { LongestTest = jsontest.CountThroughPrompt; }
                            if (accuracy > HighestAccuracy) { HighestAccuracy = accuracy; }

                            if ((int)(((test.KeystrokeCount - test.Misinputs) / 5) / (test.TimeTaken / 60)) > wpm) { numoflowerwpmtests++; }

                            if ((double)((int)((test.KeystrokeCount - test.Misinputs) / test.KeystrokeCount * 10000)) / 100 > accuracy) { numofloweraccurracytests++; }

                            tests!.Add(jsontest);
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err);
                        }
                    }
                }
            }

            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You were faster than {(double)((int)(numoflowerwpmtests / tests.Count) * 10000) / 100}% of your old tests.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{numoflowerwpmtests} tests had a WPM lower than {(int)(((test.KeystrokeCount - test.Misinputs) / 5) / (test.TimeTaken / 60))} out of {tests.Count}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Your highest speed was {HighestWPM} WPM");

            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You were more accurate than {(numofloweraccurracytests / tests.Count) * 100}% of your old tests.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{numofloweraccurracytests} tests had an accuracy lower than {(double)((int)((test.KeystrokeCount - test.Misinputs) / test.KeystrokeCount * 10000)) / 100}% out of {tests.Count}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Your highest accuracy was {HighestAccuracy}%");

            Console.ForegroundColor = ConsoleColor.White;
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