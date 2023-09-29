namespace WritingMaintainableUnitTests.Module6_UnitTestPractices.PointOfSale
{
    public class Scanner
    {
        private readonly IDisplay _display;

        public Scanner(IDisplay display)
        {
            _display = display;
        }
        
        public void Scan()
        {
            var scannedItem = Item.Cornflakes();
            _display.DisplayItem(scannedItem);
        }
    }
}