using System.Text.RegularExpressions;
using System.Diagnostics;

namespace typing
{
    internal class TypingTest
    {
        bool ContinueTest = true;

        // possibly split this into two objects, one for timed one for length

        readonly string prompt;         // for length tests 
        readonly int timer;             // for timed tests
        readonly bool ghost;            // prompt printing setting
        readonly bool autocorrect;      // when incorrect char entered, show correct char in red

        List<char> UserKeystrokes = new List<char>(); // TODO: make keystroke object containing timestamp + char for more detailed stats

        // statistics
        private double TimeTaken;       // seconds taken
        private double KeystrokeCount;  // total keystrokes
        private int CountThroughPrompt; // keystrokes through the prompt, ignoring misinputs
        private double Misinputs;       // incorrect keystrokes

        public TypingTest(string _prompt, int _timer, bool _ghost, bool _autocorrect)
        {
            prompt = _prompt;
            timer = _timer;
            ghost = _ghost;
            autocorrect = _autocorrect;
        }

        public void InputProc()
        {
            var regex = new Regex(@"[\x00 -\x7F]"); // valid ascii

            while (ContinueTest)
            {
                ConsoleKeyInfo keystroke = Console.ReadKey(true);

                KeystrokeCount++;

                if (keystroke.Key == ConsoleKey.Enter) { } // strip enter

                else if (keystroke.Key == ConsoleKey.Tab) { } // strip tab

                else if (keystroke.Key == ConsoleKey.Backspace) // special condition for backspace (TODO: add modifier support later)
                {
                    if (CountThroughPrompt > 0)
                    {
                        CountThroughPrompt--;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\b" + (ghost ? prompt[CountThroughPrompt] : " ") + "\b");
                    }
                }

                else if (keystroke.Key == ConsoleKey.Escape) { ContinueTest = false; } // end on ESC

                else if (regex.IsMatch(keystroke.KeyChar.ToString())) // check if input matches regex
                {
                    if (keystroke.KeyChar.ToString() == prompt[CountThroughPrompt].ToString()) // check for misinputs
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(keystroke.KeyChar.ToString()); // write char
                    }
                    else
                    { // TODO: flag for whether keystroke or prompt is displayed
                        Console.ForegroundColor = ConsoleColor.Red; // misinput
                        Console.Write(autocorrect ? prompt[CountThroughPrompt] : keystroke.KeyChar.ToString()); // write red char on incorrect keystroke, and "correct" char if autocorrect
                        Misinputs++;
                    }

                    CountThroughPrompt++;
                }

                if (CountThroughPrompt == prompt.Length) { ContinueTest = false; } // end on prompt finish
            }
        }

        public void RunTest()
        {
            Console.Clear();

            Stopwatch stopwatch = new Stopwatch();

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

            // TODO: add space condition for extra / too few letters per word (maybe)
            // TODO: fix end on prompt finish
            // TODO: fix backspace going up a line (idk it seems to work for some reason now)
            // TODO: add redraw on screen size change (or just end the test)

            Thread t = new Thread(new ThreadStart(InputProc));

            t.Start();
            stopwatch.Start();

            Thread.Sleep((int)timer * 1000);

            ContinueTest = false;

            t.Interrupt();

            TimeTaken = stopwatch.Elapsed.TotalSeconds;

            stopwatch.Stop();

            PrintStats();
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