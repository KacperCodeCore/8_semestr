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

timer2 = (int)stopwatch.ElapsedMilliseconds;
Console.WriteLine("\n###################### Completed");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestSolutionValue}");
Console.WriteLine($"Iteration: {currIteration}");
Console.WriteLine($"Time: {timer2 - timer1} ms\n");

#endregion Brute Force





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


timer2 = (int)stopwatch.ElapsedMilliseconds;
Console.WriteLine("###################### Simulated annealing");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestValue}");
Console.WriteLine($"Iterations: {currIteration}");
Console.WriteLine($"Time: {timer2 - timer1} ms\n");
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


timer2 = (int)stopwatch.ElapsedMilliseconds;

Console.Write($"\nIterations: {currIteration}");
Console.WriteLine($"\nBest: {bestSolutionValue}");
Console.WriteLine($"Time: {timer2 - timer1} ms\n");
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

static class ProgramGeneticAlgorithm
{
    public static Population population;
    public static int[] values;
    public static int[] weights;
    public static int[] solution;
    public static int N;
    public static int BagWeight;
    public static int Iteration = 200;
    public static Stopwatch stopwatch;


    public static void Calculate()
    {
        int timer1;
        int timer2;
        timer1 = (int)stopwatch.ElapsedMilliseconds;
        population = new Population(20, N);

        for (int i = 0; i < 200; i++)
        {
            population.Evolution();
        }


        timer2 = (int)stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"\nIteration: {Iteration}");
        Console.WriteLine($"Best: {population.population[0].Value}");
        Console.WriteLine($"Time: {timer2 - timer1} ms\n");
    }
}

class Population
{
    public Individual[] population;

    private Individual[] _nextGeneration;
    private double[] _normValues;

    private readonly Random _rnd = new();
    private readonly Random _rnd2 = new();


    public Population(int size, int numberOfItems)
    {
        population = new Individual[size];
        _nextGeneration = new Individual[size];

        // Generate random individuals (solutions)
        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new Individual(_rnd, numberOfItems);
        }

        _normValues = new double[population.Length];
    }

    public void Evolution()
    {
        Array.Sort(population, (a, b) => b.Value.CompareTo(a.Value));

        // fitness array for roulette wheel selection
        for (int i = 0; i < population.Length; i++)
        {
            _normValues[i] = (i == 0) ? population[i].Value : _normValues[i - 1] + population[i].Value;

        }

        double sum = _normValues[^1];
        _normValues = Array.ConvertAll(_normValues, x => x / sum);  // Normalize the cumulative values
        _normValues[^1] = 1.0;  // Ensure the last value is exactly 1.0

        // Copy the best individuals directly to the next generation
        for (int i = 0; i < 5; i++)
        {
            _nextGeneration[i] = population[i].Clone();
        }

        // Perform crossover to create new individuals
        for (int i = 5; i < _nextGeneration.Length; i++)
        {
            _nextGeneration[i] = Individual.Crossover(population[Sample()], population[Sample()]);
        }

        // Apply mutation to the new individuals
        for (int i = 1; i < _nextGeneration.Length; i++)
        {
            _nextGeneration[i].Mutation(0.1f);
        }

        // Replace the old population with the new one
        (population, _nextGeneration) = (_nextGeneration, population);
    }

    // Sample method: Select individuals using roulette wheel selection
    private int Sample()
    {
        double probability = _rnd2.NextDouble();
        int index = Array.BinarySearch(_normValues, probability);
        return (index < 0) ? ~index : index;
    }
}


public class Individual
{
    public int[] Solution;
    public int Value;
    public int Weight;

    private readonly Random _rnd;

    public Individual(Random rnd, int numberOfItems)
    {
        _rnd = rnd;
        Solution = new int[numberOfItems];

        for (int i = 0; i < Solution.Length; i++)
        {
            Solution[i] = _rnd.Next(2);
        }

        UpdateValueAndWeight();
    }

    public Individual Clone()
    {
        return new Individual(_rnd, Solution.Length)
        {
            Solution = (int[])Solution.Clone(),
            Value = Value,
            Weight = Weight
        };
    }

    public void Mutation(float mutationRate)
    {
        if (_rnd.NextDouble() < mutationRate)
        {
            int index = _rnd.Next(Solution.Length);
            Solution[index] = 1 - Solution[index];  // Flip the selected bit
            UpdateValueAndWeight();
        }

    }

    public static Individual Crossover(Individual parent1, Individual parent2)
    {
        var solution = new int[parent1.Solution.Length];

        // Randomly choose genes from either parent
        for (int i = 0; i < solution.Length; i++)
        {
            solution[i] = parent1._rnd.Next(2) == 0 ? parent1.Solution[i] : parent2.Solution[i];
        }

        // Return a new individual with the combined solution
        return new Individual(parent1._rnd, parent1.Solution.Length)
        {
            Solution = solution
        };
    }

    private void UpdateValueAndWeight()
    {
        Value = 0;
        Weight = 0;
        for (int i = 0; i < Solution.Length; i++)
        {
            if (Solution[i] == 1)
            {
                Value += ProgramGeneticAlgorithm.values[i];
                Weight += ProgramGeneticAlgorithm.weights[i];
            }
        }

        if (Weight > ProgramGeneticAlgorithm.BagWeight)
        {
            Value = 0;
        }
    }
}
#endregion
