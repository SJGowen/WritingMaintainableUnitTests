namespace WritingMaintainableUnitTests.Module6UnitTestPractices.PointOfSale
{
    public readonly struct Item
    {
        private readonly string _name;
        private readonly double _price;

        private Item(string name, double price)
        {
            _name = name;
            _price = price;
        }

        public static Item Cornflakes()
        {
            return new Item("Cornflakes", 3.36);
        }

        public override string ToString()
        {
            return $"{_name}: {_price}";
        }
    }
}