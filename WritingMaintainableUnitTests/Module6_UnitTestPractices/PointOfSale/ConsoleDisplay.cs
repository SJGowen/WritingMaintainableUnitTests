using System;

namespace WritingMaintainableUnitTests.Module6_UnitTestPractices.PointOfSale
{
    public interface IDisplay
    {
        void DisplayItem(Item item);
    }

    public class ConsoleDisplay : IDisplay
    {
        public void DisplayItem(Item item)
        {
            Console.WriteLine(item);
        }
    }
}