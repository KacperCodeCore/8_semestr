// Brute Force #################################################################
#region Brute Force
using System.Diagnostics;

const int N = 15; // number of object in the bag
const int BagWeight = 100; // capacity of the bag

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
int timer1 = (int)stopwatch.ElapsedMilliseconds;
int timer2;
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

int[] solutionClone = (int[])solution.Clone();

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
stopwatch.Stop();
timer2 = (int)stopwatch.ElapsedMilliseconds;
Console.WriteLine("\n###################### Completed");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestSolutionValue}");
Console.WriteLine($"Iteration: {currIteration}");
Console.WriteLine($"Time: {timer2 - timer1} ms\n");
stopwatch.Reset();
#endregion Brute Force





// Random Selection #################################################################
#region Random Selection
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
#endregion




// Simulated Annealing #################################################################
#region Simulated annealing
solutionValues.Clear();
for (int i = 0; i < N; i++) { solution[i] = 0; }
bestSolutionValue = int.MinValue;
currIteration = 0;

float temperature = 1100;
float epsilon = 0.01f;
float coolingRate = 0.995f;

int bestValue = 0;
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

stopwatch.Start();
timer1 = (int)stopwatch.ElapsedMilliseconds;
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
            bestValue = currentSolutionValue;
        }
        else
        {
            solution = (int[])prevSolution.Clone();
            currentSolutionValue = prevValue;
            currentSolutionWeight = prevWeight;
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

stopwatch.Stop();
timer2 = (int)stopwatch.ElapsedMilliseconds;
Console.WriteLine("###################### Simulated annealing");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestValue}");
Console.WriteLine($"Iterations: {currIteration}");
Console.WriteLine($"Time: {timer2 - timer1} ms\n");
stopwatch.Reset();
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


stopwatch.Start();
timer1 = (int)stopwatch.ElapsedMilliseconds;
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
    // PrintAB("");
    currIteration++;
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
        }
    }
}

stopwatch.Stop();
timer2 = (int)stopwatch.ElapsedMilliseconds;

Console.Write($"\nIterations: {currIteration}");
Console.WriteLine($"\nBest: {bestSolutionValue}");
Console.WriteLine($"Time: {timer2 - timer1} ms\n");
stopwatch.Reset();
#endregion






// Genetic Algorithm for Knapsack Problem #################################################################
#region Genetic Algorithm
Console.Write("###################### Genetic Algorithm");

currIteration = 0;
currentSolutionWeight = 0;
currentSolutionValue = 0;
bestSolutionValue = 0;
solutionValues.Clear();


ProgramGeneticAlgorithm.values = (int[])values.Clone();
ProgramGeneticAlgorithm.weights = (int[])weights.Clone();
ProgramGeneticAlgorithm.solution = (int[])solutionClone.Clone();
ProgramGeneticAlgorithm.N = N;
ProgramGeneticAlgorithm.BagWeight = BagWeight;
ProgramGeneticAlgorithm.stopwatch = stopwatch;
ProgramGeneticAlgorithm.Calculate();
#endregion

