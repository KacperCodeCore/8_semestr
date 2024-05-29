// Brute Force #################################################################
#region Brute Force
const int N = 13; // number of object in the bag
const int B = 50; // capacity of the bag

int[] values = new int[N];
int[] weights = new int[N];

int[] solution = new int[N];

Random random = new Random();

int bestSolutionValue = int.MinValue;
int[] BestSolution = new int[N];
List<int> solutionValues = new List<int>();

int currIteration = 0;

Console.WriteLine("Values:");
for (int i = 0; i < N; i++) { values[i] = random.Next(1, B / 2); Console.Write($"{values[i],2} "); }
Console.WriteLine();

Console.WriteLine("Weights:");
for (int i = 0; i < N; i++) { weights[i] = random.Next(1, B / 2); Console.Write($"{weights[i],2} "); }
Console.WriteLine();

do
{
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

Console.WriteLine("what it is? BestSolution ?:");
for (int i = 0; i < N; i++) { Console.Write($"{BestSolution[i],2} "); }
Console.WriteLine();

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

    return totalWeight <= B;
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

Console.WriteLine("###################### Completed");
Console.WriteLine($"Solution Values: {solutionValues.Count}");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestSolutionValue}");
#endregion Brute Force





// Random Selection #################################################################
#region Random Selection
bestSolutionValue = int.MinValue;
solutionValues.Clear();

for (int i = 0; i < N; i++)
{
    solution[i] = 0;
}

do
{
    if (IsValid2())
    {
        var solutionValue = SolutionValue2();
        if (solutionValue > bestSolutionValue)
        {
            solutionValues.Add(solutionValue);
            bestSolutionValue = solutionValue;
            BestSolution = (int[])solution.Clone();
        }
    }
} while (Next2());

Console.WriteLine("###################### Random");
Console.WriteLine($"Solution Values: {solutionValues.Count}");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestSolutionValue}");

bool Next2()
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

bool IsValid2()
{
    int totalWeigh = 0;
    for (int i = 0; i < N; i++)
    {
        totalWeigh += weights[i] * solution[i];
    }

    return totalWeigh <= B;
}

int SolutionValue2()
{
    int totalValue = 0;
    for (int i = 0; i < N; i++)
    {
        totalValue += values[i] * solution[i];
    }

    return totalValue;
}
#endregion Random Selection




// Simulated Annealing #################################################################
#region Simulated annealing
solutionValues.Clear();
for (int i = 0; i < N; i++) { solution[i] = 0; }
bestSolutionValue = int.MinValue;

float temperature = 1100;
float epsilon = 0.003f;
float coolingRate = 0.995f;


int currentSolutionWeight = 0;
int currentSolutionValue = 0;
for (int i = 0; i < N; i++)
{
    solution[i] = random.Next(0, 2);

    currentSolutionValue += values[i] * solution[i];
    currentSolutionWeight += weights[i] * solution[i];
}

do
{
    int prevValue = currentSolutionValue;
    int prevWeight = currentSolutionWeight;

    if (isValid3())
    {
        if (currentSolutionValue > bestSolutionValue)
        {
            solutionValues.Add(currentSolutionValue);
            bestSolutionValue = currentSolutionValue;
            BestSolution = (int[])solution.Clone();
        }
        else if (currentSolutionValue < prevValue)
        {
            double acceptanceProbability = Math.Exp(-(currentSolutionValue - prevValue) / temperature);
            Console.WriteLine($"acceptanceProbability: {acceptanceProbability}");
            if (random.NextDouble() < acceptanceProbability)
            {
                solutionValues.Add(currentSolutionValue);
                bestSolutionValue = currentSolutionValue;
                BestSolution = (int[])solution.Clone();
            }
            else
            {
                currentSolutionValue = prevValue;
                currentSolutionWeight = prevWeight;
            }
        }
        else
        {
            currentSolutionValue = prevValue;
            currentSolutionWeight = prevWeight;
        }
        temperature *= coolingRate;
    }
} while (Next3() && temperature > epsilon);


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

bool isValid3() => currentSolutionWeight <= B;

Console.WriteLine("###################### Simulated annealing");
Console.WriteLine($"Solution Values: {solutionValues.Count}");
foreach (int s in solutionValues) { Console.Write($"{s} "); }
Console.WriteLine();
Console.WriteLine($"Best: {bestSolutionValue}");

#endregion Simulated annealing