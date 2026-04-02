This code is designed to run Tank Drive robots on the CTRE HERO Board platform. 
This code can only run and function when being written from VS 2019, with the CTRE HERO Extensions installed.

It includes a ramping function, and an E-Stop function (pushing all four letter buttons- X, Y, A, and B).
The robot my default runs at half speed to avoid killing people- to double the speed, hold down either trigger.

I might be stupid but I couldn't figure out how to commit easily using the Git menu inside VS, so I wrote a script to 
add all files in the working directory, ask for a commit message, then automatically commit to the working branch.
To use it, run `./ez-commit.sh` from the command line (provided you're inside the project directory). 
