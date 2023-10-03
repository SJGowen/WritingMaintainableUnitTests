using System;

namespace WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses
{
    public class Employee
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public Address Address { get; }
        public BankInformation BankInformation { get; }

        public Employee(Guid id, string firstName, string lastName, Address address, BankInformation bankInformation)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            BankInformation = bankInformation;
        }
    }

    public class Address
    {
        public string StreetName { get; }
        public string HouseNumber { get; }
        public string PostalCode { get; }
        public string City { get; }

        public Address(string streetName, string houseNumber, string postalCode, string city)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            City = city;
        }
    }

    public class BankInformation
    {
        public string BankName { get; }
        public string IBAN { get; }

        public BankInformation(string bankName, string iban)
        {
            BankName = bankName;
            IBAN = iban;
        }
    }
}