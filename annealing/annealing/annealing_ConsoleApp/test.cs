using System;
using System.Collections.Generic;

class SimulatedAnnealingKnapsack
{
    const int N = 13; // number of objects in the bag
    const int B = 50; // capacity of the bag

    static Random random = new Random();

    static void Main(string[] args)
    {
        int[] values = new int[N];
        int[] weights = new int[N];
        int[] bestSolution = new int[N];
        double initialTemperature = 1000;
        double coolingRate = 0.99;
        double stoppingTemperature = 0.01;

        // Initialize values and weights
        Console.WriteLine("Values:");
        for (int i = 0; i < N; i++)
        {
            values[i] = random.Next(1, B / 2);
            Console.Write($"{values[i],2} ");
        }

        Console.WriteLine();
        Console.WriteLine("Weights:");
        for (int i = 0; i < N; i++)
        {
            weights[i] = random.Next(1, B / 2);
            Console.Write($"{weights[i],2} ");
        }
        Console.WriteLine();

        // Initialize the initial solution
        int[] currentSolution = GenerateInitialSolution();
        int currentSolutionValue = SolutionValue(currentSolution, values);
        int bestSolutionValue = currentSolutionValue;

        double temperature = initialTemperature;

        while (temperature > stoppingTemperature)
        {
            int[] nextSolution = GenerateNeighbor(currentSolution);
            if (IsValid(nextSolution, weights))
            {
                int nextSolutionValue = SolutionValue(nextSolution, values);
                int deltaE = nextSolutionValue - currentSolutionValue;

                if (deltaE > 0 || random.NextDouble() < Math.Exp(deltaE / temperature))
                {
                    currentSolution = nextSolution;
                    currentSolutionValue = nextSolutionValue;

                    if (currentSolutionValue > bestSolutionValue)
                    {
                        bestSolutionValue = currentSolutionValue;
                        Array.Copy(currentSolution, bestSolution, N);
                    }
                }
            }

            temperature *= coolingRate;
        }

        // Output the best solution
        Console.WriteLine("Best Solution:");
        for (int i = 0; i < N; i++)
        {
            Console.Write($"{bestSolution[i],2} ");
        }
        Console.WriteLine();
        Console.WriteLine($"Best Value: {bestSolutionValue}");
    }

    static int[] GenerateInitialSolution()
    {
        int[] solution = new int[N];
        for (int i = 0; i < N; i++)
        {
            solution[i] = random.Next(2); // 0 or 1 randomly
        }
        return solution;
    }

    static int[] GenerateNeighbor(int[] solution)
    {
        int[] neighbor = (int[])solution.Clone();
        int bit = random.Next(N);
        neighbor[bit] = 1 - neighbor[bit]; // Flip the bit
        return neighbor;
    }

    static bool IsValid(int[] solution, int[] weights)
    {
        int totalWeight = 0;
        for (int i = 0; i < N; i++)
        {
            totalWeight += weights[i] * solution[i];
        }
        return totalWeight <= B;
    }

    static int SolutionValue(int[] solution, int[] values)
    {
        int totalValue = 0;
        for (int i = 0; i < N; i++)
        {
            totalValue += values[i] * solution[i];
        }
        return totalValue;
    }
}
