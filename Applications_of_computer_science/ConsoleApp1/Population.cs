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