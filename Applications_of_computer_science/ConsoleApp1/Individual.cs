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
        //! do 1x instead for loop
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