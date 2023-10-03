using System.Linq;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder
{
    [Specification]
    public class SimilarAddressTests
    {
        [Observation]
        public void Example_of_using_a_test_data_builder_for_creating_similar_objects()
        {
            var marketStreetStoreAddress = ExampleOf.Address()
                .WithStreetName("Market Street")
                .WithPostalCode("CA 92101")
                .WithCity("San Diego");

            var storeAddresses = new Address[]
            {
                marketStreetStoreAddress.WithHouseNumber("705"),
                marketStreetStoreAddress.WithHouseNumber("707"),
                marketStreetStoreAddress.WithHouseNumber("709")
            };
            
            Assert.That(storeAddresses.Length, Is.EqualTo(3));
            
            Assert.That(storeAddresses.ElementAt(0).StreetName, Is.EqualTo("Market Street"));
            Assert.That(storeAddresses.ElementAt(0).HouseNumber, Is.EqualTo("705"));
            Assert.That(storeAddresses.ElementAt(0).PostalCode, Is.EqualTo("CA 92101"));
            Assert.That(storeAddresses.ElementAt(0).City, Is.EqualTo("San Diego"));
            
            Assert.That(storeAddresses.ElementAt(1).StreetName, Is.EqualTo("Market Street"));
            Assert.That(storeAddresses.ElementAt(1).HouseNumber, Is.EqualTo("707"));
            Assert.That(storeAddresses.ElementAt(1).PostalCode, Is.EqualTo("CA 92101"));
            Assert.That(storeAddresses.ElementAt(1).City, Is.EqualTo("San Diego"));
            
            Assert.That(storeAddresses.ElementAt(2).StreetName, Is.EqualTo("Market Street"));
            Assert.That(storeAddresses.ElementAt(2).HouseNumber, Is.EqualTo("709"));
            Assert.That(storeAddresses.ElementAt(2).PostalCode, Is.EqualTo("CA 92101"));
            Assert.That(storeAddresses.ElementAt(2).City, Is.EqualTo("San Diego"));
        }
        
        [Observation]
        public void Example_of_using_a_test_data_builder_for_creating_similar_objects_for_varying_properties_01()
        {
            var marketStreetStoreAddress = ExampleOf.Address()
                .WithStreetName("Market Street")
                .WithPostalCode("CA 92101")
                .WithCity("San Diego");

            var storeAddresses = new Address[]
            {
                marketStreetStoreAddress.WithHouseNumber("705"),    // Market Street
                marketStreetStoreAddress.WithStreetName("Sixth Ave").WithHouseNumber("212"),    // Sixth Ave 
                marketStreetStoreAddress.WithHouseNumber("709")    // Also Sixth Ave !!
            };
            
            Assert.That(storeAddresses.ElementAt(0).StreetName, Is.EqualTo("Market Street"));
            Assert.That(storeAddresses.ElementAt(1).StreetName, Is.EqualTo("Sixth Ave"));
            Assert.That(storeAddresses.ElementAt(2).StreetName, Is.EqualTo("Sixth Ave"));    // Not what we want!!
        }
        
        [Observation]
        public void Example_of_using_a_test_data_builder_for_creating_similar_objects_for_varying_properties_02()
        {
            var marketStreetStoreAddress = ExampleOf.ImmutableAddress()
                .WithStreetName("Market Street")
                .WithPostalCode("CA 92101")
                .WithCity("San Diego");

            var storeAddresses = new Address[]
            {
                marketStreetStoreAddress.WithHouseNumber("705"),    // Market Street
                marketStreetStoreAddress.WithStreetName("Sixth Ave").WithHouseNumber("212"),    // Sixth Ave 
                marketStreetStoreAddress.WithHouseNumber("709")    // Back to Market Street
            };
            
            Assert.That(storeAddresses.ElementAt(0).StreetName, Is.EqualTo("Market Street"));
            Assert.That(storeAddresses.ElementAt(1).StreetName, Is.EqualTo("Sixth Ave"));
            Assert.That(storeAddresses.ElementAt(2).StreetName, Is.EqualTo("Sixth Ave"));    // That's much better!!
        }
    }

    #region Test data builders

    public static class ExampleOf
    {
        public static AddressBuilder Address() => new AddressBuilder();
        public static ImmutableAddressBuilder ImmutableAddress() => new ImmutableAddressBuilder();
    }
    
    public class ImmutableAddressBuilder
    {
        private string _streetName;
        private string _houseNumber;
        private string _postalCode;
        private string _city;

        public ImmutableAddressBuilder()
            : this("Spooner Street", "31", "2060ABC", "Quahog")
        {}

        private ImmutableAddressBuilder(string streetName, string houseNumber, string postalCode, string city)
        {
            _streetName = streetName;
            _houseNumber = houseNumber;
            _postalCode = postalCode;
            _city = city;
        }

        public ImmutableAddressBuilder WithStreetName(string streetName)
        {
            _streetName = streetName;
            return new ImmutableAddressBuilder(streetName, _houseNumber, _postalCode, _city);
        }

        public ImmutableAddressBuilder WithHouseNumber(string houseNumber)
        {
            _houseNumber = houseNumber;
            return new ImmutableAddressBuilder(_streetName, houseNumber, _postalCode, _city);
        }

        public ImmutableAddressBuilder WithPostalCode(string postalCode)
        {
            _postalCode = postalCode;
            return new ImmutableAddressBuilder(_streetName, _houseNumber, postalCode, _city);
        }

        public ImmutableAddressBuilder WithCity(string city)
        {
            _city = city;
            return new ImmutableAddressBuilder(_streetName, _houseNumber, _postalCode, city);
        }

        public Address Build()
        {
            return new Address(_streetName, _houseNumber, _postalCode, _city);
        }
        
        public static implicit operator Address(ImmutableAddressBuilder builder)
        {
            return builder.Build();
        }
    }

    #endregion
}