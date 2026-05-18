using System;

namespace SVU_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            // =========================================================
            // PART 1: System Information 
            // =========================================================
            Console.WriteLine("Windows version: {0}", Environment.OSVersion);
            Console.WriteLine("64 Bit operating system?: {0}", Environment.Is64BitOperatingSystem ? "Yes" : "No");
            Console.WriteLine("PC Name: {0}", Environment.MachineName);
            Console.WriteLine("Number of CPUS: {0}", Environment.ProcessorCount);
            Console.WriteLine("Windows folder: {0}", Environment.SystemDirectory);
            Console.WriteLine("Logical Drives Available: {0}", String.Join(", ", Environment.GetLogicalDrives()).TrimEnd('.', '\\').Replace("\\", String.Empty));
            Console.WriteLine(new string('-', 50));

            // Display Available Operations
            Console.WriteLine("Available Operations:");
            Console.WriteLine("Addition 1\tSubtraction 2\tRemainder 3");
            Console.WriteLine("And 4\t\tOr 5\t\tNot 6");
            Console.WriteLine(new string('-', 50));

            // =========================================================
            // Ask for Number of Questions
            // =========================================================
            int numQuestions = 0;
            while (true)
            {
                Console.WriteLine("Please enter the maximum number of questions:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out numQuestions) && numQuestions > 0)
                {
                    break;
                }
                Console.WriteLine("The number of questions should be an integer > 0, please enter it again.");
            }

            // =========================================================
            // Arrays Setup 
            // =========================================================
            int[] num1Arr = new int[numQuestions];
            int[] num2Arr = new int[numQuestions];

            string[] arithOpSignArr = new string[numQuestions];
            string[] logicOpNameArr = new string[numQuestions];

            int[] correctArithArr = new int[numQuestions];
            int[] correctLogicArr = new int[numQuestions];

            int[] userArithArr = new int[numQuestions];
            int[] userLogicArr = new int[numQuestions];

            bool[] userArithIgnored = new bool[numQuestions];
            bool[] userLogicIgnored = new bool[numQuestions];

            int[] scoreArr = new int[numQuestions];

            // =========================================================
            // User Information Input
            // =========================================================
            string userInfo = "";
            string distinctChars = "";

            while (true)
            {
                Console.WriteLine("\nPlease enter your name, id number, and interests with a space between each part (Accepted Chars: A-Z a-z 0-9)");
                userInfo = Console.ReadLine();

                int validCount = 0;
                distinctChars = ""; // Reset for every new attempt

                // Manually count valid chars and extract distinct 
                for (int i = 0; i < userInfo.Length; i++)
                {
                    char c = userInfo[i];
                    bool isValid = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');

                    if (isValid)
                    {
                        validCount++;

                        // Check if already in distinctChars
                        bool found = false;
                        for (int j = 0; j < distinctChars.Length; j++)
                        {
                            if (distinctChars[j] == c)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            distinctChars += c;
                        }
                    }
                }

                if (validCount >= 6)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("The entered text should contain at least 6 of Accepted chars.");
                }
            }

            Console.WriteLine("\nYour full name and id and ...");
            Console.WriteLine(userInfo);
            Console.WriteLine("Distinct Chars are: " + distinctChars);
            Console.WriteLine(new string('-', 50));

            // =========================================================
            // Generating Questions
            // =========================================================
            Random rand = new Random();

            for (int i = 0; i < numQuestions; i++)
            {
                Console.WriteLine($"\n--- Question {i + 1} ---");

                // Generate n1 [10, 3794]
                int n1 = rand.Next(10, 3795);

                // Ensure n2 < n1 and in range [1, 300]
                int maxN2 = 300;
                if (n1 - 1 < 300)
                {
                    maxN2 = n1 - 1;
                }
                int n2 = rand.Next(1, maxN2 + 1);

                num1Arr[i] = n1;
                num2Arr[i] = n2;

                // Build Binary Strings manually for 12 bits
                string bin1 = "";
                int temp1 = n1;
                for (int b = 0; b < 12; b++)
                {
                    bin1 = (temp1 % 2) + bin1;
                    temp1 /= 2;
                }

                string bin2 = "";
                int temp2 = n2;
                for (int b = 0; b < 12; b++)
                {
                    bin2 = (temp2 % 2) + bin2;
                    temp2 /= 2;
                }

                Console.WriteLine($"Number 1: {n1} (Binary: {bin1})");
                Console.WriteLine($"Number 2: {n2} (Binary: {bin2})");

                // Select Arithmetic Operation (1: +, 2: -, 3: %)
                int arithType = rand.Next(1, 4);
                if (arithType == 1) { correctArithArr[i] = n1 + n2; arithOpSignArr[i] = "+"; }
                else if (arithType == 2) { correctArithArr[i] = n1 - n2; arithOpSignArr[i] = "-"; }
                else if (arithType == 3) { correctArithArr[i] = n1 % n2; arithOpSignArr[i] = "%"; }

                // Select Logical Operation (1: AND, 2: OR, 3: NOT)
                int logicType = rand.Next(1, 4);
                if (logicType == 1) logicOpNameArr[i] = "AND";
                else if (logicType == 2) logicOpNameArr[i] = "OR";
                else logicOpNameArr[i] = "NOT";

                // Manual Bitwise Logic Calculation using % 2 and / 2
                int logicAns = 0;
                int t1 = n1;
                int t2 = n2;
                int multiplier = 1;

                for (int b = 0; b < 12; b++)
                {
                    int bit1 = t1 % 2;
                    int bit2 = t2 % 2;
                    t1 /= 2;
                    t2 /= 2;

                    int resBit = 0;
                    if (logicType == 1) // AND
                    {
                        if (bit1 == 1 && bit2 == 1) resBit = 1;
                    }
                    else if (logicType == 2) // OR
                    {
                        if (bit1 == 1 || bit2 == 1) resBit = 1;
                    }
                    else // NOT (Only applies to n1)
                    {
                        if (bit1 == 0) resBit = 1;
                    }

                    logicAns += resBit * multiplier;
                    multiplier *= 2;
                }
                correctLogicArr[i] = logicAns;

                // Get Arithmetic Answer from User
                Console.WriteLine($"\nSolve the arithmetic operation: {n1} {arithOpSignArr[i]} {n2}");
                Console.Write("Your answer (or 'ignore'): ");
                string arithInput = Console.ReadLine();
                if (arithInput != null && arithInput.ToLower() == "ignore")
                {
                    userArithIgnored[i] = true;
                }
                else
                {
                    int parsedArith;
                    if (int.TryParse(arithInput, out parsedArith))
                    {
                        userArithArr[i] = parsedArith;
                    }
                    else
                    {
                        userArithIgnored[i] = true; // Treat invalid as ignored/wrong
                    }
                }

                // Get Logical Answer from User
                if (logicType == 3)
                {
                    Console.WriteLine($"Solve the logical operation: NOT {n1} (First 12 bits)");
                }
                else
                {
                    Console.WriteLine($"Solve the logical operation: {n1} {logicOpNameArr[i]} {n2}");
                }

                Console.Write("Your answer (or 'ignore'): ");
                string logicInput = Console.ReadLine();
                if (logicInput != null && logicInput.ToLower() == "ignore")
                {
                    userLogicIgnored[i] = true;
                }
                else
                {
                    int parsedLogic;
                    if (int.TryParse(logicInput, out parsedLogic))
                    {
                        userLogicArr[i] = parsedLogic;
                    }
                    else
                    {
                        userLogicIgnored[i] = true; // Treat invalid as ignored/wrong
                    }
                }

                // Evaluate Score (0, 1, or 2)
                int currentScore = 0;
                if (!userArithIgnored[i] && userArithArr[i] == correctArithArr[i]) currentScore++;
                if (!userLogicIgnored[i] && userLogicArr[i] == correctLogicArr[i]) currentScore++;
                scoreArr[i] = currentScore;
            }

            // =========================================================
            // PART 2: Statistics and Menu Loop
            // =========================================================
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("All questions completed. Entering statistics menu.");

            while (true)
            {
                Console.WriteLine("\nPlease choose an option:");
                Console.WriteLine("1) Show Correct Answers Percentage");
                Console.WriteLine("2) Convert a decimal number to 12-bit binary");
                Console.WriteLine("3) Display generated numbers and operations for all questions");
                Console.WriteLine("4) Display user answers vs correct answers");
                Console.WriteLine("Type 'quit' to exit.");
                Console.Write("Choice: ");

                string choice = Console.ReadLine();

                if (choice != null && choice.ToLower() == "quit")
                {
                    Console.WriteLine("Exiting program. Goodbye!");
                    break;
                }

                if (choice == "1")
                {
                    int totalPossiblePoints = numQuestions * 2;
                    int totalPointsEarned = 0;
                    for (int i = 0; i < numQuestions; i++)
                    {
                        totalPointsEarned += scoreArr[i];
                    }

                    double percentage = (totalPointsEarned * 100.0) / totalPossiblePoints;
                    Console.WriteLine($"\nPercentage of Correct Answers: {percentage}%");
                    Console.WriteLine($"Total Points: {totalPointsEarned} out of {totalPossiblePoints}");
                }
                else if (choice == "2")
                {
                    Console.Write("\nEnter a decimal number: ");
                    string decInput = Console.ReadLine();
                    int decNum;
                    if (int.TryParse(decInput, out decNum))
                    {
                        string binOut = "";
                        int tempDec = decNum;
                        for (int b = 0; b < 12; b++)
                        {
                            binOut = (tempDec % 2) + binOut;
                            tempDec /= 2;
                        }
                        Console.WriteLine($"Decimal {decNum} in 12-bit Binary is: {binOut}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid number.");
                    }
                }
                else if (choice == "3")
                {
                    Console.WriteLine("\n--- Generated Numbers and Operations ---");
                    for (int i = 0; i < numQuestions; i++)
                    {
                        string b1 = "";
                        int t1 = num1Arr[i];
                        for (int b = 0; b < 12; b++) { b1 = (t1 % 2) + b1; t1 /= 2; }

                        string b2 = "";
                        int t2 = num2Arr[i];
                        for (int b = 0; b < 12; b++) { b2 = (t2 % 2) + b2; t2 /= 2; }

                        Console.WriteLine($"Q{i + 1}: Num1 = {num1Arr[i]} ({b1}), Num2 = {num2Arr[i]} ({b2})");
                        Console.WriteLine($"     Operations -> Arith: [{arithOpSignArr[i]}], Logic: [{logicOpNameArr[i]}]");
                    }
                }
                else if (choice == "4")
                {
                    Console.WriteLine("\n--- User Answers vs Correct Answers ---");
                    for (int i = 0; i < numQuestions; i++)
                    {
                        Console.WriteLine($"Question {i + 1} Score: {scoreArr[i]} / 2");

                        string userArithStr = userArithIgnored[i] ? "Ignored" : userArithArr[i].ToString();
                        Console.WriteLine($"  Arith ({arithOpSignArr[i]}): User = {userArithStr} | Correct = {correctArithArr[i]}");

                        string userLogicStr = userLogicIgnored[i] ? "Ignored" : userLogicArr[i].ToString();
                        Console.WriteLine($"  Logic ({logicOpNameArr[i]}): User = {userLogicStr} | Correct = {correctLogicArr[i]}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }
    }
}
