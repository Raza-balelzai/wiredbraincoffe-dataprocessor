using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessor
    {
        private readonly Dictionary<string, int> _countPerCoffeeType = new();
        private readonly ICoffeeCountStore _coffeeCountStore;
        private MachineDataItem? _previousItem;

        public MachineDataProcessor(ICoffeeCountStore coffeeCountStore)
        {
            _coffeeCountStore = coffeeCountStore;
        }
        public void ProcessItems(MachineDataItem[] dataItems)
        {
            _previousItem = null;
            _countPerCoffeeType.Clear();

            foreach (var dataItem in dataItems)
            {
                ProcessItem(dataItem);
            }

            SaveCountPerCoffeeType();
        }

        private void ProcessItem(MachineDataItem dataItem)
        {
            if (IsNewerThanPreviousItem(dataItem))
            {

                if (!_countPerCoffeeType.ContainsKey(dataItem.CoffeeType))
                {
                    _countPerCoffeeType.Add(dataItem.CoffeeType, 1);
                }
                else
                {
                    _countPerCoffeeType[dataItem.CoffeeType]++;
                }
                _previousItem = dataItem;

            }

        }

        private bool IsNewerThanPreviousItem(MachineDataItem dataItem)
        {
            return _previousItem is null || _previousItem.CreatedAt < dataItem.CreatedAt;
        }

        private void SaveCountPerCoffeeType()
        {
            foreach (var entry in _countPerCoffeeType)
            {
                _coffeeCountStore.Save(new CoffeeCountItem(entry.Key, entry.Value));
            }
        }
    }
}
