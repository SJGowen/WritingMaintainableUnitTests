using System;
using System.Collections.Generic;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

public static class Example
{
    public static ExpenseSheetBuilder ExpenseSheet() 
        => new ExpenseSheetBuilder();
    
    public static AddressBuilder Address() 
        => new AddressBuilder();
    public static BankInformationBuilder BankInformation() 
        => new BankInformationBuilder();
    public static EmployeeBuilder Employee() 
        => new EmployeeBuilder();
    
    public static HeadOfDepartmentBuilder HeadOfDepartment() 
        => new HeadOfDepartmentBuilder();
    public static ChiefFinancialOfficerBuilder ChiefFinancialOfficer() 
        => new ChiefFinancialOfficerBuilder();
}

public class ExpenseSheetBuilder
{
    private Guid _id;
    private EmployeeBuilder _employee;
    private DateTime _submissionDate;
    private List<Expense> _expenses;

    public ExpenseSheetBuilder()
    {
        _id = Guid.NewGuid();
        _employee = Example.Employee();
        _submissionDate = new DateTime(2018, 10, 01);
        _expenses = new List<Expense>();
    }

    public ExpenseSheetBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ExpenseSheetBuilder WithEmployee(Action<EmployeeBuilder> build)
    {
        build(_employee);
        return this;
    }
    
    public ExpenseSheetBuilder WithEmployee(EmployeeBuilder employeeBuilder)
    {
        _employee = employeeBuilder;
        return this;
    }

    public ExpenseSheetBuilder WithSubmissionDate(DateTime submissionDate)
    {
        _submissionDate = submissionDate;
        return this;
    }

    public ExpenseSheetBuilder WithExpense(decimal amount, DateTime date, string description)
    {
        var expense = new Expense(amount, date, description);
        _expenses.Add(expense);
        return this;
    }

    public ExpenseSheet Build()
    {
        return new ExpenseSheet(_id, _employee, _submissionDate, _expenses);
    }
    
    public static implicit operator ExpenseSheet(ExpenseSheetBuilder builder)
    {
        return builder.Build();
    }
}

public class EmployeeBuilder
{
    private Guid _id;
    private string _firstName;
    private string _lastName;
    private AddressBuilder _address;
    private BankInformationBuilder _bankInformation;

    public EmployeeBuilder()
    {
        _id = Guid.NewGuid();
        _firstName = "Peter";
        _lastName = "Griffin";
        _address = Example.Address();
        _bankInformation = Example.BankInformation();
    }

    public EmployeeBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public EmployeeBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public EmployeeBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public EmployeeBuilder WithAddress(Action<AddressBuilder> build)
    {
        build(_address);
        return this;
    }

    public EmployeeBuilder WithBankInformation(Action<BankInformationBuilder> build)
    {
        build(_bankInformation);
        return this;
    }

    public Employee Build()
    {
        return new Employee(_id, _firstName, _lastName, _address, _bankInformation);
    }
    
    public static implicit operator Employee(EmployeeBuilder builder)
    {
        return builder.Build();
    }
}

public class AddressBuilder
{
    private string _streetName;
    private string _houseNumber;
    private string _postalCode;
    private string _city;

    public AddressBuilder()
    {
        _streetName = "Spooner Street";
        _houseNumber = "31";
        _postalCode = "2060ABC";
        _city = "Quahog";
    }

    public AddressBuilder WithStreetName(string streetName)
    {
        _streetName = streetName;
        return this;
    }

    public AddressBuilder WithHouseNumber(string houseNumber)
    {
        _houseNumber = houseNumber;
        return this;
    }

    public AddressBuilder WithPostalCode(string postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    public AddressBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public Address Build()
    {
        return new Address(_streetName, _houseNumber, _postalCode, _city);
    }
    
    public static implicit operator Address(AddressBuilder builder)
    {
        return builder.Build();
    }
}

public class BankInformationBuilder
{
    private string _bankName;
    private string _iban;

    public BankInformationBuilder()
    {
        _bankName = "ING";
        _iban = "BE68844010370034";
    }

    public BankInformationBuilder WithBankName(string bankName)
    {
        _bankName = bankName;
        return this;
    }

    public BankInformationBuilder WithIBAN(string iban)
    {
        _iban = iban;
        return this;
    }

    public BankInformation Build()
    {
        return new BankInformation(_bankName, _iban);
    }
    
    public static implicit operator BankInformation(BankInformationBuilder builder)
    {
        return builder.Build();
    }
}

public class HeadOfDepartmentBuilder
{
    private Guid _id;
    
    public HeadOfDepartmentBuilder()
    {
        _id = Guid.NewGuid();
    }

    public HeadOfDepartmentBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public HeadOfDepartment Build()
    {
        return new HeadOfDepartment(_id);
    }
    
    public static implicit operator HeadOfDepartment(HeadOfDepartmentBuilder builder)
    {
        return builder.Build();
    }
}

public class ChiefFinancialOfficerBuilder
{
    private Guid _id;
    
    public ChiefFinancialOfficerBuilder()
    {
        _id = Guid.NewGuid();
    }

    public ChiefFinancialOfficerBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ChiefFinancialOfficer Build()
    {
        return new ChiefFinancialOfficer(_id);
    }
    
    public static implicit operator ChiefFinancialOfficer(ChiefFinancialOfficerBuilder builder)
    {
        return builder.Build();
    }
}