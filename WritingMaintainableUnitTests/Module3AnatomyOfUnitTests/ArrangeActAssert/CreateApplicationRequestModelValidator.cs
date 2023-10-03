namespace WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssert;

public static class CreateApplicationRequestModelValidator
{
    public static bool IsValid(CreateApplicationRequestModel model)
    {
        if (model.FirstName == null || model.LastName == null)
            return false;

        return true;
    }
}

public class CreateApplicationRequestModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}