public interface IObserver
{
    void Update(float temperature);
}

public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}


public class WeatherStation : ISubject
{
    private List<IObserver> observers;
    private float temperature;

    public WeatherStation()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature);
        }
    }

    public void SetTemperature(float temperature)
    {
        this.temperature = temperature;
        NotifyObservers();
    }
}


public class TemperatureDisplay : IObserver
{
    private float temperature;

    public void Update(float temperature)
    {
        this.temperature = temperature;
        Display();
    }

    public void Display()
    {
        Console.WriteLine("Current temperature is: " + temperature + "°C");
    }
}



public class TemperatureLogger : IObserver
{
    public void Update(float temperature)
    {
        Log(temperature);
    }

    public void Log(float temperature)
    {
        Console.WriteLine("Logging temperature: " + temperature + "°C");
    }
}

public class JustPrintTemperature : IObserver
{
    public void Update(float temperature)
    {
        Log(temperature);
    }

    public void Log(float temperature)
    {
        Console.WriteLine(temperature + "°C");
    }
}



public class Program
{
    public static void Main(string[] args)
    {
        WeatherStation weatherStation = new WeatherStation();

        TemperatureDisplay display = new TemperatureDisplay();
        TemperatureLogger logger = new TemperatureLogger();
        JustPrintTemperature justPrint = new JustPrintTemperature();

        weatherStation.RegisterObserver(display);
        weatherStation.RegisterObserver(logger);
        weatherStation.RegisterObserver(justPrint);

        weatherStation.SetTemperature(25.3f);
        weatherStation.SetTemperature(44.6f);

        Console.WriteLine();
        weatherStation.RemoveObserver(display);
        weatherStation.RemoveObserver(justPrint);
        weatherStation.SetTemperature(27.5f);
    }
}
