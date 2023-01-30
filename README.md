# typing

a simple c# typing test and scorekeeper

## how to use

download the ZIP for your platform

you need the wordlist_10000.txt if you'd like to use --random

open a command line, and run the program with the flags you'd like!

## how to build

install dotnet runtime 7.0

run `build.bat` on windows or `build.sh` on linux

find the program inside `/bin/release/net7.0/linux-x64/publish/your-platform/`

## flags

```
--autocorrect

shows the correct character you should have typed, instead of what you did type

--ghost

you type overtop the prompt, instead of below

--random

generates a random prompt from a wordlist

--prompt "{string}"

use {string} as your prompt

--timer {int}

set a time limit 

--length {int}

set a word length limit (defaults to 25 with --random)
```

## todo

- add --file flag for wordlists / prompts

- fix windows exit handling

- save tests & compare PBs