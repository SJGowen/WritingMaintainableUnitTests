namespace WritingMaintainableUnitTests.Module1TypesOfTests.CascadingFailure;

public interface IAuthorizationService
{
    bool IsAllowed(CustomerAction customerAction);
}

public class AuthorizationServiceV2 : IAuthorizationService
{
    private readonly UserContext _userContext;

    public AuthorizationServiceV2(UserContext userContext)
    {
        _userContext = userContext;
    }

    public bool IsAllowed(CustomerAction customerAction)
    {
        if (_userContext.Role == UserRole.Unknown)
            return false;

        if (_userContext.Role == UserRole.HelpDeskStaff && customerAction == CustomerAction.MakePreferred)
            return false;

        // ...

        return true;
    }
}