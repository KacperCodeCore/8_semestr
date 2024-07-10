
// Flyweight pattern for Tower
public class TowerFlyweight
{
    public string Color { get; private set; }
    public string Shape { get; private set; }
    public string Type { get; private set; }
    public int Power { get; private set; }

    public TowerFlyweight(string color, string shape, string type, int power)
    {
        Color = color;
        Shape = shape;
        Type = type;
        Power = power;
    }

    public void Display(int x, int y)
    {
        Console.WriteLine($"Totem {Shape} in color {Color} at position ({x}, {y}) type {Type} power {Power}.");
    }
}




public class TowerFactory
{
    private Dictionary<string, TowerFlyweight> _totems = new Dictionary<string, TowerFlyweight>();

    public TowerFlyweight GetTotem(string color, string shape, string type, int power)
    {
        string key = $"{color}_{shape}_{type}_{power}";
        if (!_totems.ContainsKey(key))
        {
            _totems[key] = new TowerFlyweight(color, shape, type, power);
        }
        return _totems[key];
    }
}




// Chain of Responsibility pattern for Tower actions
public abstract class TowerHandler
{
    protected TowerHandler? nextHandler;

    public void SetNextHandler(TowerHandler handler)
    {
        if (nextHandler == null)
        {
            nextHandler = handler;
        }
        else
        {
            nextHandler.SetNextHandler(handler);
        }
    }

    public abstract void Handle(Tower totem, string actionType);

    public virtual void DisplaySummary()
    {
        nextHandler?.DisplaySummary();
    }
}




public class DefenseHandler : TowerHandler
{
    public override void Handle(Tower totem, string actionType)
    {
        if (actionType == "Defense" && Program.BlocksToDefend > 0)
        {
            Program.BlocksToDefend -= totem.Flyweight.Power;
            Console.WriteLine($"Tower at position ({totem.X}, {totem.Y}) is defending for {totem.Flyweight.Power}. Remaining blocks to defend: {Program.BlocksToDefend}");
        }
        else if (nextHandler != null)
        {
            nextHandler.Handle(totem, actionType);
        }
    }

    public override void DisplaySummary()
    {
        Console.WriteLine($"Remaining blocks to defend: {Program.BlocksToDefend}");
        base.DisplaySummary();
    }
}




public class AttackHandler : TowerHandler
{
    public override void Handle(Tower totem, string actionType)
    {
        if (actionType == "Attack" && Program.AttacksToDestroy > 0)
        {
            Program.AttacksToDestroy -= totem.Flyweight.Power;
            Console.WriteLine($"Tower at position ({totem.X}, {totem.Y}) is attacking for {totem.Flyweight.Power}. Remaining attacks to destroy: {Program.AttacksToDestroy}");
        }
        else if (nextHandler != null)
        {
            nextHandler.Handle(totem, actionType);
        }
    }

    public override void DisplaySummary()
    {
        Console.WriteLine($"Remaining attacks to destroy: {Program.AttacksToDestroy}");
        base.DisplaySummary();
    }
}




public class Tower
{
    public TowerHandler Handler { get; private set; }
    public TowerFlyweight Flyweight { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }


    public Tower(TowerFlyweight flyweight, int x, int y)
    {
        Flyweight = flyweight;
        X = x;
        Y = y;

        // Setting the right handler based on the tower type
        if (Flyweight.Type == "Defense")
        {
            Handler = new DefenseHandler();
        }
        else if (Flyweight.Type == "Attack")
        {
            Handler = new AttackHandler();
        }
        else
        {
            throw new ArgumentException($"Unknown totem type: {Flyweight.Type}");
        }
        Handler.Handle(this, Flyweight.Type);
    }

    public void PerformAction()
    {
        Handler.Handle(this, Flyweight.Type);
    }
}




class Program
{
    public static int BlocksToDefend = 10;
    public static int AttacksToDestroy = 10;

    static void Main(string[] args)
    {
        Console.WriteLine($"Blocks To Defend: {BlocksToDefend}");
        Console.WriteLine($"Attacks To Destroy: {AttacksToDestroy}");
        Console.WriteLine();

        // Create TowerFactory
        TowerFactory factory = new TowerFactory();

        // Tower Flyweights
        TowerFlyweight attackTower1 = factory.GetTotem("red", "circle", "Attack", 1);
        TowerFlyweight defenseTower1 = factory.GetTotem("blue", "circle", "Defense", 1);
        TowerFlyweight defenseTower3 = factory.GetTotem("red", "square", "Defense", 3);

        // Create ToWers using factory
        // every tower is also next handler for chain of responsibility
        Tower totem1 = new Tower(attackTower1, 0, 0);
        Tower totem2 = new Tower(defenseTower3, 1, 1);
        Tower totem3 = new Tower(defenseTower1, 3, 2);
        Tower totem4 = new Tower(defenseTower1, 5, 2);

        // Display summary
        Console.WriteLine();
        string defendText = BlocksToDefend > 0 ? "You didn't defend your ship" : "You defended your ship";
        string AttackText = AttacksToDestroy > 0 ? "You didn't destroy the enemy ship" : "you destroyed the enemy ship";
        Console.WriteLine($"Blocks To Defend: {BlocksToDefend}, {defendText}");
        Console.WriteLine($"Attacks To Destroy: {AttacksToDestroy}, {AttackText}");
    }
}

