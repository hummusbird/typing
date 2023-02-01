using System.Text.RegularExpressions;
using System.Diagnostics;

namespace typing
{
    internal class TypingTest
    {
        // possibly split this into two objects, one for timed one for length

        public readonly string prompt;         // for length tests 
        public readonly int timer;             // for timed tests
        readonly bool ghost;            // prompt printing setting
        readonly bool autocorrect;      // when incorrect char entered, show correct char in red

        List<char> UserKeystrokes = new List<char>(); // TODO: make keystroke object containing timestamp + char for more detailed stats

        // statistics

        public double TimeTaken { get; private set; }          // seconds taken
        public double KeystrokeCount { get; private set; }     // total keystrokes
        public int CountThroughPrompt { get; private set; }    // keystrokes through the prompt, ignoring misinputs
        public double Misinputs { get; private set; }          // incorrect keystrokes

        public TypingTest(string _prompt, int _timer, bool _ghost, bool _autocorrect, double timetaken = 0, double keystrokecount = 0, int countthroughprompt = 0, double misinputs = 0)
        {
            prompt = _prompt;
            timer = _timer;
            ghost = _ghost;
            autocorrect = _autocorrect;

            // used for deserialization
            TimeTaken = timetaken;
            KeystrokeCount = keystrokecount;
            CountThroughPrompt = countthroughprompt;
            Misinputs = misinputs;
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
            // TODO: fix backspace going up a line (idk it seems to work for some reason now)
            // TODO: add redraw on screen size change (or just end the test)
            // TODO: fix windows colour change on exit

            var regex = new Regex(@"[\x00 -\x7F]"); // valid ascii

            bool ContinueTest = true;

            stopwatch.Start();

            while (ContinueTest && (timer <= 0 ? true : stopwatch.Elapsed.TotalSeconds < timer))
            {
                if (Console.KeyAvailable)
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
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // misinput
                            Console.Write(autocorrect ? prompt[CountThroughPrompt] : keystroke.KeyChar.ToString()); // write red char on incorrect keystroke, and "correct" char if autocorrect
                            Misinputs++;
                        }

                        CountThroughPrompt++;
                    }

                    if (CountThroughPrompt == prompt.Length) { ContinueTest = false; } // end on prompt finish
                }
            }

            ContinueTest = false;

            TimeTaken = stopwatch.Elapsed.TotalSeconds;

            stopwatch.Stop();
        }
    }
}