namespace WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssert;

public class ApplicationRequest
{
    public string Name { get; }
    public ApplicationRequestStatus Status { get; private set; }

    public ApplicationRequest(string firstName, string lastName)
    {
        Name = $"{firstName} {lastName}";
    }

    public void Approve()
    {
        Status = ApplicationRequestStatus.Approved;
    }
}

public enum ApplicationRequestStatus
{
    Pending = 0,
    Approved = 1
}