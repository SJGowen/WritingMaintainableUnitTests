namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices.TestDataBuilders.Banking;

public static class Example
{
    public static ActiveAccountBuilder ActiveAccount() => new ActiveAccountBuilder();
    public static FrozenAccountBuilder FrozenAccount() => new FrozenAccountBuilder();
    public static BankCardBuilder BankCard() => new BankCardBuilder(); 
}