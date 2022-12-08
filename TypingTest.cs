using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace typing
{
    internal class TypingTest
    {
        // possibly split this into two objects, one for timed one for length

        readonly string prompt;         // for length tests 
        readonly int timer;             // for timed tests
        readonly bool ghost;            // prompt printing setting

        List<char> UserKeystrokes = new List<char>(); // TODO: make keystroke object containing timestamp + char for more detailed stats

        // statistics
        private double TimeTaken;       // seconds taken
        private double KeystrokeCount;  // total keystrokes
        private int CountThroughPrompt; // keystrokes through the prompt, ignoring misinputs
        private double Misinputs;       // incorrect keystrokes

        public TypingTest(string _prompt, int _timer, bool _ghost)
        {
            prompt = _prompt;
            timer = _timer;
            ghost = _ghost;

            RunTest();
            PrintStats();
        }

        private void RunTest()
        {
            Console.Clear();
            DateTimeOffset StartTime = DateTime.Now;
            (int Left, int Top) CursorStart = Console.GetCursorPosition();

            if (ghost)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(prompt);
                Console.SetCursorPosition(CursorStart.Left, CursorStart.Top);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(prompt);
            }

            int currentIndex = 0;
            bool ContinueTest = true;

            var regex = new Regex(@"[\x00 -\x7F]"); // valid ascii

            while (ContinueTest)
            {
                // TODO: add space condition for extra / too few letters per word (maybe)
                // TODO: add timer & timed end condition
                // TODO: fix backspace going up a line

                ConsoleKeyInfo keystroke = Console.ReadKey(true);

                KeystrokeCount++;

                if (keystroke.Key == ConsoleKey.Enter) { } // strip enter
                else if (keystroke.Key == ConsoleKey.Tab) { } // strip tab

                else if (keystroke.Key == ConsoleKey.Backspace) // special condition for backspace (TODO: add modifier support later)
                {
                    if (currentIndex > 0)
                    {
                        currentIndex--;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\b" + (ghost ? prompt[currentIndex] : " ") + "\b");
                    }
                }

                else if (keystroke.Key == ConsoleKey.Escape) { ContinueTest = false; } // end on ESC

                else if (regex.IsMatch(keystroke.KeyChar.ToString())) // check if input matches regex
                {
                    if (keystroke.KeyChar.ToString() == prompt[currentIndex].ToString()) // check for misinputs
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(keystroke.KeyChar.ToString()); // write char
                    }
                    else
                    { // TODO: flag for whether keystroke or prompt is displayed
                        Console.ForegroundColor = ConsoleColor.Red; // misinput
                        Console.Write(prompt[currentIndex]); // write invalid char
                        Misinputs++;
                    }

                    currentIndex++;
                }

                if (currentIndex == prompt.Length) { ContinueTest = false; } // end on prompt finish
            }

            DateTimeOffset EndTime = DateTime.Now;

            CountThroughPrompt = currentIndex; // use this instead of prompt.Length in case test is cut short or timed

            TimeTaken = (double)(EndTime.ToUnixTimeMilliseconds() - StartTime.ToUnixTimeMilliseconds()) / 1000;
        }

        private void PrintStats() // TODO: get PB results and compare / update
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"TIME:       {TimeTaken} seconds");
            Console.WriteLine($"LENGTH:     {CountThroughPrompt} characters");
            Console.WriteLine($"WPM:        {(int)(((KeystrokeCount - Misinputs) / 5) / (TimeTaken / 60))}"); // WPM = (Keystrokes / 5) / Time taken (minutes)
            Console.WriteLine($"RAW WPM:    {(int)((KeystrokeCount / 5) / (TimeTaken / 60))}"); // passing to (int) removes decimal places
            Console.WriteLine($"ACCURACY:   {(double)((int)((KeystrokeCount - Misinputs) / KeystrokeCount * 10000)) / 100}%"); // 2 decimal places
            Console.WriteLine($"MISINPUTS:  {Misinputs}/{KeystrokeCount}");
        }
    }
}