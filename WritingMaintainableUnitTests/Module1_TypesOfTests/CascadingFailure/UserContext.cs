namespace WritingMaintainableUnitTests.Module1_TypesOfTests.CascadingFailure
{
    public class UserContext
    {
        public UserRole Role { get; }

        public UserContext(UserRole role)
        {
            Role = role;
        }
    }

    public enum UserRole
    {
        Unknown = 0,
        HelpDeskStaff = 1,
        BackOfficeManager = 2
    }
}