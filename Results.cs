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

            string filename = DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".json";

            if (!Directory.Exists("typing-results")) { Directory.CreateDirectory("typing-results"); } // create results folder if one doesn't exist

            using (StreamWriter SW = new StreamWriter("typing-results/" + filename)) // save the test under the current date & time
            {
                SW.WriteLine(JsonConvert.SerializeObject(test, Formatting.Indented));
                Console.WriteLine($"{filename} saved!");
            }
        }

        public static void CompareAgainstTests(TypingTest test)
        {
            if (!Directory.Exists("typing-results")) { Directory.CreateDirectory("typing-results"); }

            string[] files = Directory.GetFiles("typing-results", "*.json");

            int[] milestones = { 5, 10, 25, 50, 69, 100, 420, 1000, 10000 };

            if (files.Length < 1) { Console.WriteLine("\nCongrats on completing your first test!"); return; } // prevent rest of stats being printed

            if (milestones.Contains(files.Length + 1))
            {
                string suffix = "th";
                if ((files.Length).ToString().EndsWith("1")) { suffix = "st"; }
                else if ((files.Length).ToString().EndsWith("2")) { suffix = "nd"; }
                else if ((files.Length).ToString().EndsWith("3")) { suffix = "rd"; }
                Console.WriteLine($"\nCongratulations on beating your {files.Length + 1}{suffix} test!");
            }

            List<TypingTest>? tests = new List<TypingTest>();

            double numoflowerwpmtests = 0;
            double numofloweraccurracytests = 0;

            int testwpm = (int)(((test.KeystrokeCount - test.Misinputs) / 5) / (test.TimeTaken / 60));
            double testaccuracy = (double)((int)((test.KeystrokeCount - test.Misinputs) / test.KeystrokeCount * 10000)) / 100;

            double HighestWPM = testwpm;
            double LongestTest = 0;
            double HighestAccuracy = testaccuracy;

            foreach (string filename in files)
            {
                using (StreamReader SR = new StreamReader(filename))
                {
                    try
                    {
                        string json = SR.ReadToEnd();

                        TypingTest jsontest = JsonConvert.DeserializeObject<TypingTest>(json)!;

                        int wpm = (int)(((jsontest.KeystrokeCount - jsontest.Misinputs) / 5) / (jsontest.TimeTaken / 60));
                        double accuracy = (double)((int)((jsontest.KeystrokeCount - jsontest.Misinputs) / jsontest.KeystrokeCount * 10000)) / 100;

                        if (testwpm > wpm) { numoflowerwpmtests++; }
                        if (testaccuracy > accuracy) { numofloweraccurracytests++; }

                        if (wpm > HighestWPM) { HighestWPM = wpm; }
                        if (jsontest.CountThroughPrompt > LongestTest) { LongestTest = jsontest.CountThroughPrompt; }
                        if (accuracy > HighestAccuracy) { HighestAccuracy = accuracy; }

                        tests!.Add(jsontest);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err);
                    }
                }

            }

            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You were faster than {(double)((int)((numoflowerwpmtests / tests.Count) * 10000)) / 100}% of your old tests.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{numoflowerwpmtests} out of {tests.Count} tests had a WPM lower than {testwpm}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Your highest speed was {HighestWPM} WPM");

            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You were more accurate than {(double)((int)((numofloweraccurracytests / tests.Count) * 10000)) / 100}% of your old tests.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{numofloweraccurracytests} out of {tests.Count} tests had an accuracy lower than {testaccuracy}%");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Your highest accuracy was {HighestAccuracy}%");

            if (LongestTest == test.CountThroughPrompt) { Console.WriteLine($"\nThis tied for your longest test, at {LongestTest} characters!"); }
            else if (LongestTest < test.CountThroughPrompt) { Console.WriteLine($"\nThis was your longest test, at {LongestTest} characters!"); }

            Console.ForegroundColor = ConsoleColor.White;
        }

        // improvement since first test
        // streak counter?
        // did you know
        // delete folder to restart
        // facts idk

        // "you cheated" message
    }
}