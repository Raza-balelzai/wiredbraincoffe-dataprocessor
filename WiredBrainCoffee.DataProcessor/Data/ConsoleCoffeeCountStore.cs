using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data;

public class ConsoleCoffeeCountStore : ICoffeeCountStore
{
    private readonly TextWriter textWriter;
    public ConsoleCoffeeCountStore() : this(Console.Out) { }

    public ConsoleCoffeeCountStore(TextWriter textWriter)
    {
        this.textWriter = textWriter;
    }
    public void Save(CoffeeCountItem item)
    {
        var line = $"{item.CoffeeType}:{item.Count}";
        textWriter.WriteLine(line);
    }
}
