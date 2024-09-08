//
using System;

class Program
{
    const int N = 8;
    const int B = 20;

    static int[] values = new int[N];
    static int[] weights = new int[N];

    static int[] solution = new int[N];

    static Random random = new Random(N);

    static int bestSolutionValue = int.MinValue;
    static int[] BestSolution = new int[N];


    static void Main()
    {
        //ustalanie i wypisywanie ddanych
        {
            Console.WriteLine("Values:");
            for (int i = 0; i < N; i++)
            {
                values[i] = random.Next(1, B / 2);
                Console.Write($"{values[i]}, ");
            }
            Console.WriteLine();
            Console.WriteLine("Weights:");
            for (int i = 0; i < N; i++)
            {
                weights[i] = random.Next(1, B / 2);
                Console.Write($"{weights[i]}, ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        do
        {
            if (IsValid())
            {
                var solutionValue = SolutionValue();
                if (solutionValue > bestSolutionValue)
                {
                    bestSolutionValue = solutionValue;
                    BestSolution = (int[])solution.Clone();
                }
            }
        } while (Next());

        Console.WriteLine($"Best Solution Value: {bestSolutionValue}");
        Console.Write("Best Solution: ");
        for (int i = 0; i < N; i++)
        {
            Console.Write($"{BestSolution[i]}, ");
        }
        Console.WriteLine();
    }

    static bool Next()
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

    static bool IsValid()
    {
        int totalWeight = 0;
        for (int i = 0; i < N; i++)
        {
            totalWeight += weights[i] * solution[i];
        }
        return totalWeight <= B;
    }

    static int SolutionValue()
    {
        int totalValue = 0;
        for (int i = 0; i < N; i++)
        {
            totalValue += values[i] * solution[i];
        }
        return totalValue;
    }
}
