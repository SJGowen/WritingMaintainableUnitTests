using NUnit.Framework;
using WritingMaintainableUnitTests.Module6UnitTestPractices.PointOfSale;

namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices._03_SelfShunt._01_WithSelfShunt
{
    public class ScannerTests : IDisplay
    {
        [Test]
        public void ScanTest()
        {
            var scanner = new Scanner(this);
            scanner.Scan();

            Assert.That(_displayedItem, Is.EqualTo(Item.Cornflakes()));
        }
        
        public void DisplayItem(Item item)
        {
            _displayedItem = item;
        }

        private Item _displayedItem;
    }
}