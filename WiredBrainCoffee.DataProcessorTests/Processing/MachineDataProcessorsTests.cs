using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessorsTests : IDisposable
{
    private readonly FakeCoffeCountStore _fakeCoffeCountStore;
    private readonly MachineDataProcessor _machineDataProcessor;

    public MachineDataProcessorsTests()
    {
        _fakeCoffeCountStore = new();
        _machineDataProcessor = new(_fakeCoffeCountStore);
    }
    [Fact]
    public void ShouldSaveCountPerCoffeeType()
    {
        //Arrange 

        var items = new[]
        {
            new MachineDataItem("Cappuccino",new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Cappuccino",new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Espresso",new DateTime(2022,10,27,10,0,0))
        };
        //Act
        _machineDataProcessor.ProcessItems(items);
        //Assert
        Assert.Equal(2, _fakeCoffeCountStore.SavedItems.Count);
        CoffeeCountItem item = _fakeCoffeCountStore.SavedItems[0];
        Assert.Equal(2, item.Count);
        Assert.Equal("Cappuccino", item.CoffeeType);

        item = _fakeCoffeCountStore.SavedItems[1];
        Assert.Equal(1, item.Count);
        Assert.Equal("Espresso", item.CoffeeType);


    }

    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer()
    {
        //Arrange 
        var items = new[]
        {

            new MachineDataItem("Cappuccino",new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Cappuccino",new DateTime(2022,10,27,7,0,0)), // older then the upper one
            new MachineDataItem("Cappuccino",new DateTime(2022,10,27,7,10,0)), //still older then the first one
            new MachineDataItem("Cappuccino",new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Espresso",new DateTime(2022,10,27,10,0,0)),
            new MachineDataItem("Espresso",new DateTime(2022,10,27,10,0,0)) //should be ignored coz its the same instance as the last one
        };
        //Act
        _machineDataProcessor.ProcessItems(items);
        //Assert
        Assert.Equal(2, _fakeCoffeCountStore.SavedItems.Count);
        CoffeeCountItem item = _fakeCoffeCountStore.SavedItems[0];
        Assert.Equal(2, item.Count);
        Assert.Equal("Cappuccino", item.CoffeeType);

        item = _fakeCoffeCountStore.SavedItems[1];
        Assert.Equal(1, item.Count);
        Assert.Equal("Espresso", item.CoffeeType);


    }


    [Fact]
    public void ShouldClearPreviousCoffeeCount()
    {
        //Arrange 
        FakeCoffeCountStore fakeCoffeCountStore = new();
        MachineDataProcessor machineDataProcessor = new(fakeCoffeCountStore);
        var items = new[]
        {
            new MachineDataItem("Cappuccino",new DateTime(2022,10,27,8,0,0)),
        };
        //Act
        machineDataProcessor.ProcessItems(items);
        machineDataProcessor.ProcessItems(items);

        //Assert
        Assert.Equal(2, fakeCoffeCountStore.SavedItems.Count);
        foreach (var item in fakeCoffeCountStore.SavedItems)
        {
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    public void Dispose()
    {
        //This runs after every test 
    }
}

public class FakeCoffeCountStore : ICoffeeCountStore
{
    public List<CoffeeCountItem> SavedItems { get; } = new();

    public void Save(CoffeeCountItem item)
    {
        SavedItems.Add(item);
    }
}

