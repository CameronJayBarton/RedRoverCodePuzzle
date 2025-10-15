# RedRoverCodePuzzle
My name is Cameron Barton and this console app is my response to the Red Rover Code Puzzle assignment found at https://github.com/RedRoverK12/Red-Rover-Code-Puzzle.

Check out my LinkedIn profile and connect with me at https://www.linkedin.com/in/cameronbarton/.

# How to Run the Program
## Option 1: Run Published Executable
Navigate to the Releases menu in this GitHub repository to download the latest executable that matches the hardware architecture of your machine. If there is not an executable published for the hardware architecture of your machine let me know and I will publish one for your desired hardware architecture! I can also create a container image if you prefer.

Grant access to execute the file using a command such as ```chmod +x filename```. Run the executable file from your terminal with the required arguments.

Example 1 (no sort):
```
RedRoverCodePuzzle.exe "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)" "none"
```

Example 2 (alphabetical sort):
```
RedRoverCodePuzzle.exe "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)" "alpha"
```

## Option 2: Run Code Directly
1. Download and install .NET 9 from https://dotnet.microsoft.com/en-us/download.
2. Download the code repository that contains this README.md file to your local machine.
3. In your terminal change the directory to the location of the root folder of this code repository.
4. Use the dotnet run command. (See examples below for this step.)

Example 1 (no sort):
```
dotnet run --project RedRoverCodePuzzle "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)" "none"
```

Example 2 (alphabetical sort):
```
dotnet run --project RedRoverCodePuzzle "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)" "alpha"
```

# Parameters
1. (REQUIRED) "Input String": This is the string you wish the program to transform according to the puzzle assignment description.

Example:
```
"(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)"
```

2. (REQUIRED) "Sorting Strategy": This is the sorting strategy you wish to employ.

Example 1 (use this to achieve the output of the first example of the puzzle assignment, meaning the input order is preserved):
```
"none"
```

Example 2 (use this to achieve the output of the second example of the puzzle assignment, meaning the output items are sorted in alphabetical ascending order for every level of every branch of the tree):
```
"alpha"
```

# Design
1. Program.cs: Orchestrates the configuration of the program based on user input parameters.
2. StringTransformer.cs: The core logic of the program which transforms an input string to an output string.
3. Node.cs: The class that represents a parsed node of the input string, consisting of a string value and references to its parent node and child nodes.
4. SortingStrategies: Sorting algorithms for ordering the parsed nodes.

# Assumptions
Some requirement assumptions were made throughout the program.

A few examples of these assumptions:
1. Leading and trailing whitespace in node values are always stripped, but any whitespace in the middle of a value is preserved.
2. Any node value that is completely empty or whitespace is skipped.
3. The node value of "null" is interpreted as a string literal of the word "null", not as a missing value.
4. The levels of nesting, number of characters, and number of separators are restricted by maximum values. You will receive a friendly error message if you violate any of these assumptions.

You can read the commented source code and automated tests to understand other assumptions.

# Feedback
Whether for the purpose of helping me improve or for the purpose of seeing changes I can implement, feedback is appreciated. Thank you for your consideration!