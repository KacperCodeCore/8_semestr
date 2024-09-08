using System;

abstract class Product
{
    public abstract float GetPrice();
    public abstract string GetDescription();
}

class ConcreteProduct : Product
{
    private float _price;
    private string _description;

    public ConcreteProduct(float price, string description)
    {
        _price = price;
        _description = description;
    }

    public override float GetPrice()
    {
        return _price;
    }

    public override string GetDescription()
    {
        return _description;
    }
}

abstract class Decorator : Product
{
    protected Product _product;

    public Decorator(Product product)
    {
        _product = product;
    }

    public override float GetPrice()
    {
        return _product.GetPrice();
    }

    public override string GetDescription()
    {
        return _product.GetDescription();
    }
}

class MascotDecorator : Decorator
{
    public MascotDecorator(Product product) : base(product) { }

    public override float GetPrice()
    {
        Console.WriteLine(_product.GetPrice());
        return _product.GetPrice();
    }

    public override string GetDescription()
    {
        return $"{_product.GetDescription()}, Store Mascot";
    }
}

class LanyardDecorator : Decorator
{
    public LanyardDecorator(Product product) : base(product) { }

    public override float GetPrice()
    {
        Console.WriteLine(_product.GetPrice());
        return _product.GetPrice() + 1;
    }

    public override string GetDescription()
    {
        return $"{_product.GetDescription()}, USB Lanyard";
    }
}

class DiscountDecorator : Decorator
{
    public DiscountDecorator(Product product) : base(product) { }

    public override float GetPrice()
    {
        Console.WriteLine(_product.GetPrice());
        return Math.Max(0, _product.GetPrice() - 10);
    }

    public override string GetDescription()
    {
        
        return $"{_product.GetDescription()}, 10 PLN Discount";
    }
}

class AdditionalShippingDecorator : Decorator
{
    public AdditionalShippingDecorator(Product product) : base(product) { }

    public override float GetPrice()
    {
        Console.WriteLine(_product.GetPrice());
        return _product.GetPrice() + 13;
    }

    public override string GetDescription()
    {
        return $"{_product.GetDescription()}, Additional Shipping Cost";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Product basicProduct = new ConcreteProduct(50, "Basic Product");

        // Adding decorators to the basic product
        Product productWithMascot = new MascotDecorator(basicProduct);
        Product productWithLanyard = new LanyardDecorator(productWithMascot);
        Product productWithDiscount = new DiscountDecorator(productWithLanyard);
        Product productWithShipping = new AdditionalShippingDecorator(productWithDiscount);

        // Printing the final price and description of the product
        Console.WriteLine($"Price: {productWithShipping.GetPrice()} PLN");
        Console.WriteLine($"Description: {productWithShipping.GetDescription()}");
    }
}
