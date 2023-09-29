namespace WritingMaintainableUnitTests.Module3_AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass
{
    public class User
    {
        public string Email { get; }
        public string Password { get; }

        public User(string email)
        {
            Email = email;
            Password = "temporary_password";
        }
    }
}