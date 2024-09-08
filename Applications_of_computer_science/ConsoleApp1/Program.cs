// Brute Force #################################################################
#region Brute Force
const int N = 7; // number of object in the bag
const int BagWeight = 100; // capacity of the bag

int[] values = new int[N];
int[] weights = new int[N];

int[] solution = new int[N];

Random random = new Random();

int bestSolutionValue = int.MinValue;
int[] BestSolution = new int[N];
List<int> solutionValues = new List<int>();

int currIteration = 0;

Console.WriteLine("Values:");
for (int i = 0; i < N; i++) { values[i] = random.Next(1, BagWeight); Console.Write($"{values[i],2} "); }
Console.WriteLine();

Console.WriteLine("Weights:");
for (int i = 0; i < N; i++) { weights[i] = random.Next(1, BagWeight); Console.Write($"{weights[i],2} "); }
Console.WriteLine();


do
{
    currIteration++;
    if (isValid())
    {
        var solutionValue = SolutionValue();
        if (solutionValue > bestSolutionValue)
        {
            solutionValues.Add(solutionValue);
            bestSolutionValue = solutionValue;
            BestSolution = (int[])solution.Clone();
        }
    }
} while (Next());

// Console.WriteLine("Start Bits:");
// for (int i = 0; i < N; i++) { Console.Write($"{BestSolution[i],2} "); }
// Console.WriteLine();

bool Next()
{
    int bit = 0;
    while (true)
    {
        if (solution[bit] == 0)
        {
            solution[bit] = 1;
            break;
        }
        else
        {
            solution[bit] = 0;
            bit++;
            if (bit == N) { return false; }
        }
    }

    return true;
}

bool isValid()
{
    int totalWeight = 0;
    for (int i = 0; i < N; i++)
    {
        totalWeight += weights[i] * solution[i];
    }

    return totalWeight <= BagWeight;
}

int SolutionValue()
{
    int totalValue = 0;
    for (int i = 0; i < N; i++)
    {
        totalValue += values[i] * solution[i];
    }

    return totalValue;
}

Console.WriteLine("\n###################### Completed");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestSolutionValue}");
Console.WriteLine($"Iteration: {currIteration}\n");
#endregion Brute Force





// // Random Selection #################################################################
// #region Random Selection
// bestSolutionValue = int.MinValue;
// solutionValues.Clear();
// currIteration = 0;
// int currentSolutionWeight = 0;
// int currentSolutionValue = 0;

// for (int i = 0; i < N; i++)
// {
//     solution[i] = 0;
// }

// do
// {
//     currIteration++;
//     if (IsValid2())
//     {
//         var solutionValue = SolutionValue2();
//         if (solutionValue > bestSolutionValue)
//         {
//             solutionValues.Add(solutionValue);
//             bestSolutionValue = solutionValue;
//             BestSolution = (int[])solution.Clone();
//         }
//     }
// } while (Next2());

// Console.WriteLine("###################### Random");
// Console.WriteLine($"Solution Values: {solutionValues.Count}");
// foreach (int s in solutionValues) { Console.Write($"{s} "); }
// Console.WriteLine();
// Console.WriteLine($"Best: {bestSolutionValue}");

// bool Next2()
// {
//     // int bit = 0;
//     // while (true)
//     // {
//     //     if (solution[bit] == 0)
//     //     {
//     //         solution[bit] = 1;
//     //         break;
//     //     }
//     //     else
//     //     {
//     //         solution[bit] = 0;
//     //         bit++;
//     //         if (bit == N) { return false; }
//     //     }
//     // }
//     var bit = random.Next(N);
//     if (solution[bit] == 1)
//     {
//         solution[bit] = 0;
//         currentSolutionValue -= values[bit];
//         currentSolutionWeight -= weights[bit];
//     }
//     else
//     {
//         solution[bit] = 1;
//         currentSolutionValue += values[bit];
//         currentSolutionWeight += weights[bit];
//     }

//     return true;
// }

// bool IsValid2()
// {
//     int totalWeigh = 0;
//     for (int i = 0; i < N; i++)
//     {
//         totalWeigh += weights[i] * solution[i];
//     }

//     return totalWeigh <= B;
// }

// int SolutionValue2()
// {
//     int totalValue = 0;
//     for (int i = 0; i < N; i++)
//     {
//         totalValue += values[i] * solution[i];
//     }

//     return totalValue;
// }
// #endregion Random Selection




// Simulated Annealing #################################################################
#region Simulated annealing
solutionValues.Clear();
for (int i = 0; i < N; i++) { solution[i] = 0; }
bestSolutionValue = int.MinValue;
currIteration = 0;

float temperature = 1100;
float epsilon = 0.01f;
float coolingRate = 0.995f;


int currentSolutionWeight = 0;
int currentSolutionValue = 0;
int prevValue = currentSolutionValue;
int prevWeight = currentSolutionWeight;
int[] prevSolution = new int[N];

for (int i = 0; i < N; i++)
{
    solution[i] = random.Next(0, 2);

    currentSolutionValue += values[i] * solution[i];
    currentSolutionWeight += weights[i] * solution[i];
}

do
{
    currIteration++;
    if (isValid3())
    {


        double delta = currentSolutionValue - prevValue;
        // double acceptanceProbability = Math.Exp(delta / temperature);
        double acceptanceProbability = CalculateAcceptanceProbability(delta, temperature);

        if (random.NextDouble() < acceptanceProbability)
        {
            prevSolution = (int[])solution.Clone();
            prevValue = currentSolutionValue;
            prevWeight = currentSolutionWeight;

            solutionValues.Add(currentSolutionValue);
        }
        else
        {
            solution = (int[])prevSolution.Clone();
            currentSolutionValue = prevValue;
            currentSolutionWeight = prevWeight;

            // solutionValues.Add(prevValue);

            // currentSolutionValue = prevValue;
            // currentSolutionWeight = prevWeight;
        }



        temperature *= coolingRate;


    }
} while (Next3() && temperature > epsilon);

double CalculateAcceptanceProbability(double delta, double temperature)
{
    if (delta > 0)
    {
        return 1.0;
    }
    else
    {
        return Math.Exp(delta / temperature);
    }
}


bool Next3()
{
    var bit = random.Next(N);
    if (solution[bit] == 1)
    {
        solution[bit] = 0;
        currentSolutionValue -= values[bit];
        currentSolutionWeight -= weights[bit];
    }
    else
    {
        solution[bit] = 1;
        currentSolutionValue += values[bit];
        currentSolutionWeight += weights[bit];
    }
    temperature *= 1 - epsilon;

    return temperature > epsilon;
}

bool isValid3() => currentSolutionWeight <= BagWeight;

Console.WriteLine("###################### Simulated annealing");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {solutionValues.Last()}");
Console.WriteLine($"Iterations: {currIteration}\n");

#endregion




// Branch and Bound #################################################################
#region Branch and Bound
Console.Write("###################### Branch and Bound");

List<int> A = new List<int>();
List<int> B = new List<int>();
List<int> BestA = new List<int>();

currIteration = 0;
currentSolutionWeight = 0;
currentSolutionValue = 0;
bestSolutionValue = 0;
solutionValues.Clear();
for (int i = 0; i < N; i++)
{
    solution[i] = 0;
}
for (int i = 0; i < N; i++)
{
    B.Add(i);
}



BB();

void PrintAB(String sign)
{
    Console.Write($"\n{sign}A: ");
    foreach (var item in A)
    {
        Console.Write(item + " ");
    }
    foreach (var item in B)
    {
        Console.Write("  ");
    }
    Console.Write("  B: ");
    foreach (var item in B)
    {
        Console.Write(item + " ");
    }
    foreach (var item in A)
    {
        Console.Write("  ");
    }
    Console.Write(" ");

    PrintSolution();

    Console.Write($" currentSolutionValue: {currentSolutionValue.ToString().PadRight(5)}  currentSolutionWeight: {currentSolutionWeight.ToString().PadRight(5)}");

}


void PrintSolution()
{
    for (int i = 0; i < N; i++)
    {
        Console.Write(solution[i]);
        Console.Write(" ");
    }
}

void BagPacking(int bit)
{
    if (solution[bit] == 1)
    {
        solution[bit] = 0;
        currentSolutionValue -= values[bit];
        currentSolutionWeight -= weights[bit];
    }
    else
    {
        solution[bit] = 1;
        currentSolutionValue += values[bit];
        currentSolutionWeight += weights[bit];
    }
}



bool Oracle()
{
    if (currentSolutionWeight > BagWeight)
    {
        return false;
    }
    return true;
}

void BB()
{
    PrintAB("");
    currIteration++;
    // // PrintAB();
    if (B.Count == 0)
    {
        Console.Write($"\nbestSolutionValue: {bestSolutionValue}");
        if (currentSolutionValue > bestSolutionValue)
        {
            bestSolutionValue = currentSolutionValue;
        }
    }
    else
    {
        int n = B.Count;
        for (int i = 0; i < n; i++)
        {
            int x = B[0];
            A.Add(x);
            B.RemoveAt(0);
            BagPacking(x);
            // PrintAB("+");

            if (Oracle())
            {
                if (currentSolutionValue > bestSolutionValue)
                {
                    bestSolutionValue = currentSolutionValue;
                }
                BB();
            }

            x = A[A.Count - 1];
            A.RemoveAt(A.Count - 1);
            B.Add(x);
            BagPacking(x);
            // PrintAB("-");



            // timeOut--;
            // if(timeOut <= 0){break;}
        }
    }
}
Console.Write($"\nIterations: {currIteration}");
Console.WriteLine($"\nBest: {bestSolutionValue}\n");
#endregion






// Genetic Algorithm #################################################################
#region Genetic Algorithm
Console.Write("###################### Genetic Algorithm");




#endregion






