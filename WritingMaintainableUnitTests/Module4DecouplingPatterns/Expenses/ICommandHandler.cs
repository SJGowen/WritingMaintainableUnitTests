namespace WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;

public interface ICommandHandler<TCommand>
{
    Result Handle(TCommand command);
}