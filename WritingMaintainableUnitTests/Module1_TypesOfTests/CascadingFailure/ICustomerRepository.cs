namespace WritingMaintainableUnitTests.Module1_TypesOfTests.CascadingFailure
{
    public interface ICustomerRepository
    {
        Customer Get(int id);
        void Save(Customer customer);
    }
}