using System.Diagnostics;

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
        stopwatch.Start();
        population = new Population(20, N);

        for (int i = 0; i < 200; i++)
        {
            population.Evolution();
        }

        stopwatch.Stop();
        Console.WriteLine($"\nIteration: {Iteration}");
        Console.WriteLine($"Best: {population.population[0].Value}");
        Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms\n");
    }
}