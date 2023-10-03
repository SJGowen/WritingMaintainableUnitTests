using NUnit.Framework;
using WritingMaintainableUnitTests.Module6UnitTestPractices.PointOfSale;

namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices._03_SelfShunt._02_WithoutSelfShunt
{
    public class ScannerTests
    {
        [Test]
        public void ScanTest()
        {
            var display = new DisplaySpy();
            var scanner = new Scanner(display);
            
            scanner.Scan();

            Assert.That(display.DisplayedItem, Is.EqualTo(Item.Cornflakes()));
        }
    }

    public class DisplaySpy : IDisplay
    {
        public Item DisplayedItem { get; private set; }

        public void DisplayItem(Item item)
        {
            DisplayedItem = item;
        }
    }
}