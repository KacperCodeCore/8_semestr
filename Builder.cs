//Builder
public class Computer
{
    public string Name { get; set; } = "";
    public string Processor { get; set; } = "";
    public string RAM { get; set; } = "";
    public string Storage { get; set; } = "";
    public string GraphicsCard { get; set; } = "";
    public string PowerSupply { get; set; } = "";
    public string Case { get; set; } = "";

    public override string ToString()
    {
        return $"Computer details:\n" +
           $"{PrintIfNotEmpty("Computer name", Name)}" +
           $"{PrintIfNotEmpty("Processor", Processor)}" +
           $"{PrintIfNotEmpty("RAM", RAM)}" +
           $"{PrintIfNotEmpty("Storage", Storage)}" +
           $"{PrintIfNotEmpty("GraphicsCard", GraphicsCard)}" +
           $"{PrintIfNotEmpty("PowerSupply", PowerSupply)}" +
           $"{PrintIfNotEmpty("Case", Case)}";
    }

    private string PrintIfNotEmpty(string propertyName, string propertyValue)
    {
        return !string.IsNullOrEmpty(propertyValue) ? $"  {propertyName}: {propertyValue}\n" : "";
    }
}




public interface IComputerBuilder
{
    void SetName();
    void SetProcessor();
    void SetRAM();
    void SetStorage();
    void SetGraphicsCard();
    void SetPowerSupply();
    void SetCase();
    Computer GetComputer();
}




public class GamingComputerBuilder : IComputerBuilder
{
    private Computer _computer = new Computer();

    public void SetName()
    {
        _computer.Name = "Gaming Computer";
    }

    public void SetProcessor()
    {
        _computer.Processor = "Intel Core i13";
    }

    public void SetRAM()
    {
        _computer.RAM = "2048GB DDR6";
    }

    public void SetStorage()
    {
        _computer.Storage = "1ZB SSD";
    }

    public void SetGraphicsCard()
    {
        _computer.GraphicsCard = "NVIDIA GeForce RTX 100000";
    }

    public void SetPowerSupply()
    {
        _computer.PowerSupply = "1000W";
    }

    public void SetCase()
    {
        _computer.Case = "Black + Led";
    }

    public Computer GetComputer()
    {
        return _computer;
    }
}




public class OfficeComputerBuilder : IComputerBuilder
{
    private Computer _computer = new Computer();

    public void SetName()
    {
        _computer.Name = "OfficeComputer";
    }

    public void SetProcessor()
    {
        _computer.Processor = "Intel Core i5";
    }

    public void SetRAM()
    {
        _computer.RAM = "16GB DDR4";
    }

    public void SetStorage()
    {
        _computer.Storage = "512GB SSD";
    }

    public void SetGraphicsCard()
    {
        _computer.GraphicsCard = "Integrated Graphics";
    }

    public void SetPowerSupply()
    {
        _computer.PowerSupply = "500W";
    }

    public void SetCase()
    {
        _computer.Case = "Standard";
    }

    public Computer GetComputer()
    {
        return _computer;
    }
}




public class CustomComputerBuilder : IComputerBuilder
{
    private Computer _computer = new Computer();

    public void SetName()
    {
        _computer.Name = "Custom Computer";
    }

    public void SetProcessor()
    {
        _computer.Processor = "Custom Processor";
    }

    public void SetRAM()
    {
        _computer.RAM = "Custom RAM";
    }

    public void SetStorage()
    {
        _computer.Storage = "Custom Storage";
    }

    public void SetGraphicsCard()
    {
        _computer.GraphicsCard = "Custom GraphicsCard";
    }

    public void SetPowerSupply()
    {
        _computer.PowerSupply = "Custom PowerSupply";
    }

    public void SetCase()
    {
        _computer.Case = "Custom Case";
    }

    public Computer GetComputer()
    {
        return _computer;
    }
}





public class ComputerDirector
{
    private IComputerBuilder _builder;

    public ComputerDirector(IComputerBuilder builder)
    {
        _builder = builder;
    }

    public void ConstructComputer()
    {
        _builder.SetName();
        _builder.SetProcessor();
        _builder.SetRAM();
        _builder.SetStorage();
        _builder.SetGraphicsCard();
        _builder.SetPowerSupply();
        _builder.SetCase();
    }

    public Computer GetComputer()
    {
        return _builder.GetComputer();
    }
}




class Program
{
    static void Main(string[] args)
    {
        IComputerBuilder gamingBuilder = new GamingComputerBuilder();
        ComputerDirector director = new ComputerDirector(gamingBuilder);
        director.ConstructComputer();
        Computer gamingComputer = director.GetComputer();
        Console.WriteLine(gamingComputer);

        IComputerBuilder officeBuilder = new OfficeComputerBuilder();
        director = new ComputerDirector(officeBuilder);
        director.ConstructComputer();
        Computer officeComputer = director.GetComputer();
        Console.WriteLine(officeComputer);

        IComputerBuilder builder = new CustomComputerBuilder();
        builder.SetName();
        builder.SetProcessor();
        builder.SetRAM();
        Console.WriteLine(builder.GetComputer());
    }
}
