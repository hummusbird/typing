# typing

a simple c# typing test and scorekeeper

## how to use

download the ZIP for your platform

you need the wordlist_10000.txt if you'd like to use --random

open a command line, and run the program with the flags you'd like!

## how to build

install dotnet runtime 7.0

run `build.bat` on windows, `build_mac.sh` on macOS or `build.sh` on linux

find the binary inside `/bin/release/net7.0/[YOUR-PLATFORM]/publish/[YOUR-PLATFORM]/`

## flags

```
Arguments:

-h, --help                       This message

-r, --random                     Generates a prompt using randomly selected words from wordlist_10000.txt

-t [int], --timer [int]          Limit the test to [int] seconds

-l [int],--length [int]          Limit the test to [int] words

-i, --incognito                  Prevent saving of results - no files or data is overwritten

-g, --ghost                      Type overtop the prompt, instead of below

-a, --autocorrect,               Incorrectly inputted characters are displayed as the correct character, in red

-p [string], --prompt [string]   Specify a prompt to be used.

-f [file], --file [file]         Load prompt from file
```

## todo

- fix windows exit handling
- check end line terminal fallback on specific terminals (seems fine on urxvt but not macOS terminal)