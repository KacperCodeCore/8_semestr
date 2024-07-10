// Interfejs Flyweight
public interface IFlyweight
{
    void Operation(int extrinsicState);
}

// Konkretny Flyweight
public class ConcreteFlyweight : IFlyweight
{
    private string _intrinsicState;

    public ConcreteFlyweight(string intrinsicState)
    {
        _intrinsicState = intrinsicState;
    }

    public void Operation(int extrinsicState)
    {
        Console.WriteLine($"Flyweight: IntrinsicState = {_intrinsicState}, ExtrinsicState = {extrinsicState}");
    }
}




public class FlyweightFactory
{
    private Dictionary<string, IFlyweight> _flyweights = new Dictionary<string, IFlyweight>();

    public IFlyweight GetFlyweight(string key)
    {
        if (!_flyweights.ContainsKey(key))
        {
            _flyweights[key] = new ConcreteFlyweight(key);
        }

        return _flyweights[key];
    }

    public void ListFlyweights()
    {
        Console.WriteLine($"FlyweightFactory: liczba flyweight'ów = {_flyweights.Count}");
        foreach (var key in _flyweights.Keys)
        {
            Console.Write($"{key} ");
        }
    }
}




public class Client
{
    public static void Main(string[] args)
    {
        FlyweightFactory factory = new FlyweightFactory();

        var flyweight1 = factory.GetFlyweight("A");
        flyweight1.Operation(1);

        var flyweight2 = factory.GetFlyweight("B");
        flyweight2.Operation(2);

        var flyweight3 = factory.GetFlyweight("A");
        flyweight3.Operation(3);

        factory.ListFlyweights();
    }
}
