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
        int timer1;
        int timer2;
        stopwatch.Start();
        timer1 = (int)stopwatch.ElapsedMilliseconds;
        population = new Population(20, N);

        for (int i = 0; i < 200; i++)
        {
            population.Evolution();
        }

        stopwatch.Stop();
        timer2 = (int)stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"\nIteration: {Iteration}");
        Console.WriteLine($"Best: {population.population[0].Value}");
        Console.WriteLine($"Time: {timer2 - timer1} ms\n");
        stopwatch.Reset();
    }
}