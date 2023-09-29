using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace WritingMaintainableUnitTests.Tests.Module5_AssertionsAndObservations._02_OnlyAssertsShouldFailTest
{
    public class SpreadSheetTests
    {
        [Test]
        public void Unit_test_that_does_not_always_fail_by_assert()
        {
            var sut = new SpreadSheet();
            sut.AddColumn("Z", "Column Z");

            var addedColumn = sut.Columns.Single(column => column.Name == "Z");
            Assert.That(addedColumn.Description, Is.EqualTo("Column Z"));
        }
        
        [Test]
        public void Unit_test_that_always_fails_by_assert()
        {
            var sut = new SpreadSheet();
            sut.AddColumn("Z", "Column Z");

            var addedColumn = sut.Columns.SingleOrDefault(column => column.Name == "Z");
            Assert.That(addedColumn?.Description, Is.EqualTo("Column Z"));
        }
    }

    public class SpreadSheet
    {
        private readonly List<Column> _columns;
        public IEnumerable<Column> Columns => _columns;

        public SpreadSheet()
        {
            _columns = new List<Column>();
        }
        
        public void AddColumn(string name, string description)
        {
            var newColumn = new Column(name, description);
            _columns.Add(newColumn);
        }       
    }

    public class Column
    {
        public string Name { get; }
        public string Description { get; }

        public Column(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}