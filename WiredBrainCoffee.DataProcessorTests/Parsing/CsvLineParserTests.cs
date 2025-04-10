using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing
{
    public class CsvLineParserTests
    {
        [Fact]
        public void ShouldParseValidLine()
        {
            //Try not to write any Act after the assert.The best flow is given below.

            //Arrange
            string[] csvLines = { "Cappuccino;10/27/2022 8:15:43 AM" };

            //Act
            MachineDataItem[] machineDataItems = CsvLineParser.Parse(csvLines);

            //Assert
            Assert.NotNull(machineDataItems);
            Assert.Single(machineDataItems);
            Assert.Equal("Cappuccino", machineDataItems[0].CoffeeType);
            Assert.Equal(new DateTime(2022, 10, 27, 8, 15, 43), machineDataItems[0].CreatedAt);
        }
        [Fact]
        public void ShouldSkipEmptyLines()
        {
            //Arrange 
            string[] csvLines = { "", " " };

            //Act 
            MachineDataItem[] machineDataItem = CsvLineParser.Parse(csvLines);

            //Assert 
            Assert.NotNull(machineDataItem);
            Assert.Empty(machineDataItem);
        }
        [InlineData("cappuccino", "Invalid CSV Line")]
        [InlineData("cappuccino;InvalidDateTime", "Invalid DateTime in CSV Line")]
        [Theory]
        public void ShouldThrowExceptionForInvalidLine(string csvLine,string expectedMessagePrefix)
        {
            //Arrange 
            string[] CsvLine = { csvLine };

            //Act and Assert in a single line 
            var exception=Assert.Throws<Exception>(()=> CsvLineParser.Parse(CsvLine));

            //Assert
            Assert.Equal($"{expectedMessagePrefix}: {csvLine}", exception.Message);
        }
    }
}
