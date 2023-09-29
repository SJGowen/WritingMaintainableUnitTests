namespace WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses
{
    public interface ICommandHandler<TCommand>
    {
        Result Handle(TCommand command);
    }
}