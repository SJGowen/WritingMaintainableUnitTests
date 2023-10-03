using System;
using WritingMaintainableUnitTests.Module6UnitTestPractices.Banking;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module6UnitTestPractices.TestDataBuilders.Banking;

namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices._01_TestBaseClass.WithoutBaseClass;

[Specification]
public class When_issuing_a_new_bank_card
{
    [Because]
    public void Of()
    {
        _result = BankCard.IssueNewBankCard();
    }

    [Observation]
    public void Then_the_bank_card_should_be_active()
    {
        _result.Blocked.Should_be_false();
    }

    private BankCard _result;
}

[Specification]
public class When_a_bank_card_is_reported_stolen
{
    [Establish]
    public void Context()
    {
        _sut = Example.BankCard();
    }

    [Because]
    public void Of()
    {
        _sut.ReportStolen();
    }
    
    [Observation]
    public void Then_the_bank_card_should_be_blocked()
    {
        _sut.Blocked.Should_be_true();
    }

    private BankCard _sut;
}

[Specification]
public class When_a_bank_card_is_expired
{
    [Establish]
    public void Context()
    {
        _sut = Example.BankCard();
    }

    [Because]
    public void Of()
    {
        _sut.Expire();
    }
    
    [Observation]
    public void Then_the_bank_card_should_be_blocked()
    {
        _sut.Blocked.Should_be_true();
    }

    private BankCard _sut;
}

[Specification]
public class When_making_a_payment
{
    [Establish]
    public void Context()
    {
        _fromAccount = Example.ActiveAccount()
            .WithAccountName("From account")
            .WithBalance(2000);
        
        _toAccount = Example.ActiveAccount()
            .WithAccountName("To account")
            .WithBalance(1000);

        _sut = Example.BankCard();
    }

    [Because]
    public void Of()
    {
        _sut.MakePayment(_fromAccount, _toAccount, 354.76);        
    }

    [Observation]
    public void Then_the_specified_amount_should_be_withdrawn_from_one_account()
    {
        _fromAccount.Balance.Should_be_equal_to(1645.24);
    }
    
    [Observation]
    public void Then_the_specified_amount_should_be_deposited_to_another_account()
    {
        _toAccount.Balance.Should_be_equal_to(1354.76);
    }

    private ActiveAccount _fromAccount;
    private ActiveAccount _toAccount;
    private BankCard _sut;
}

[Specification]
public class When_making_a_payment_using_a_blocked_bank_card
{
    [Establish]
    public void Context()
    {
        _fromAccount = Example.ActiveAccount()
            .WithAccountName("From account")
            .WithBalance(2000);
        
        _toAccount = Example.ActiveAccount()
            .WithAccountName("To account")
            .WithBalance(1000);

        _sut = Example.BankCard().AsBlocked();
    }

    [Because]
    public void Of()
    {
        _makePayment = () => _sut.MakePayment(_fromAccount, _toAccount, 162.88);
    }

    [Observation]
    public void Then_the_payment_should_not_be_allowed()
    {
        _makePayment.Should_throw_an<InvalidOperationException>();
    }

    private ActiveAccount _fromAccount;
    private ActiveAccount _toAccount;
    private BankCard _sut;
    private Action _makePayment;
}