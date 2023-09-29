namespace WritingMaintainableUnitTests.Module3_AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass
{
    public interface IUserRepository
    {
        User GetByEmail(string email);
        void Save(User user);
    }
}