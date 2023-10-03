namespace WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass
{
    public interface IEmailSender
    {
        void Send(EmailMessage message);
    }

    public class EmailMessage
    {
        public string Email { get; }
        public string Subject { get; }
        public string Message { get; }

        public EmailMessage(string email, string subject, string message)
        {
            Email = email;
            Subject = subject;
            Message = message;
        }
    }
}