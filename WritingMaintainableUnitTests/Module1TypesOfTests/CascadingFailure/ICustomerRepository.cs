namespace WritingMaintainableUnitTests.Module1TypesOfTests.CascadingFailure;

public interface ICustomerRepository
{
    Customer Get(int id);
    void Save(Customer customer);
}