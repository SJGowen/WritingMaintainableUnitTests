namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices.TestDataBuilders.BankingComposition
{
    public static class Example
    {
        public static ActiveAccountBuilder ActiveAccount() => new ActiveAccountBuilder();
        public static FrozenAccountBuilder FrozenAccount() => new FrozenAccountBuilder();
    }
}