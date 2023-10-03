namespace WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass;

public interface IUserRepository
{
    User GetByEmail(string email);
    void Save(User user);
}