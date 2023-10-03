namespace WritingMaintainableUnitTests.Module1TypesOfTests.CascadingFailure
{
    public class Customer
    {
        public int Id { get; }
        public string Email { get; private set; }
        public CustomerType Type { get; private set; }

        public Customer(int id, string email)
        {
            Id = id;
        }

        public void MakePreferred()
        {
            Type = CustomerType.Preferred;
        }

        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
        }
    }

    public enum CustomerType
    {
        Regular = 0,
        Preferred = 1
    }
}