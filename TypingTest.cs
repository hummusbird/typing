using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace typing
{
    internal class TypingTest
    {
        readonly string prompt;
        readonly int timer;

        List<char> UserKeystrokes = new List<char>();

        //statiscics
        private double TimeTaken;
        private double KeystrokeCount;
        private double Misinputs;

        public TypingTest(string _prompt, int _timer)
        {
            prompt = _prompt;
            timer = _timer;

            RunTest();

            Console.ReadLine();
        }

        private void RunTest()
        {
            DateTimeOffset StartTime = DateTime.Now;
            Console.WriteLine(prompt);
            int currentIndex = 0;

            var regex = new Regex(@"[\x00 -\x7F]"); // valid ascii


            while (true)
            {
                ConsoleKeyInfo keystroke = Console.ReadKey(true);

                KeystrokeCount++;

                if (keystroke.Key == ConsoleKey.Enter) { } // strip enter
                else if (keystroke.Key == ConsoleKey.Tab) { } // strip tab

                else if (keystroke.Key == ConsoleKey.Backspace) // special condition for backspace (possibly add modifier support later)
                {
                    if (currentIndex > 0)
                    {
                        currentIndex--;
                        Console.Write("\b \b");
                    }
                }

                else if (regex.IsMatch(keystroke.KeyChar.ToString())) // check if input matches regex
                {
                    if (keystroke.KeyChar.ToString() == prompt[currentIndex].ToString()) // check for misinputs
                    { Console.ForegroundColor = ConsoleColor.White; }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // misinput
                        Misinputs++;
                    }

                    Console.Write(keystroke.KeyChar.ToString()); // write char to console
                    currentIndex++;
                }

                if (currentIndex == prompt.Length) // end of test
                {
                    DateTimeOffset EndTime = DateTime.Now;

                    TimeTaken = (double)(EndTime.ToUnixTimeMilliseconds() - StartTime.ToUnixTimeMilliseconds()) / 1000;

                    PrintStats();

                    break;
                }
            }
        }

        private void PrintStats()
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n");
            Console.WriteLine($"TIME:       {TimeTaken} seconds");
            Console.WriteLine($"LENGTH:     {prompt.Length} characters");
            Console.WriteLine($"WPM:        {(int)(((KeystrokeCount - Misinputs) / 5) / (TimeTaken / 60))}"); // WPM = Keystrokes / 5 / Time taken (minutes)
            Console.WriteLine($"RAW WPM:    {(int)((KeystrokeCount / 5) / (TimeTaken / 60))}"); // passing to (int) removes decimal places
            Console.WriteLine($"ACCURACY:   {(double)((int)((KeystrokeCount - Misinputs) / KeystrokeCount * 10000)) / 100}%"); // 2 decimal places
            Console.WriteLine($"MISINPUTS:  {Misinputs}/{KeystrokeCount}");
        }
    }
}