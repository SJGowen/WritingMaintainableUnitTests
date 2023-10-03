namespace WritingMaintainableUnitTests.Module1_TypesOfTests.StateVerification;

using System;
using System.Collections.Generic;

public class Resume
{
    private readonly IList<Experience> _experiences;
    public IEnumerable<Experience> Experiences => _experiences;

    public Resume()
    {
        _experiences = new List<Experience>();
    }

    public void AddExperience(string employer, string role, DateTime from, DateTime until)
    {            
        var newExperience = new Experience(employer, role, from, until);
        _experiences.Add(newExperience);
    }
}

public class Experience
{
    public string Employer { get; }
    public string Role { get; }
    public DateTime From { get; }
    public DateTime Until { get; }

    public Experience(string employer, string role, DateTime from, DateTime until)
    {
        Employer = employer;
        Role = role;
        From = from;
        Until = until;
    }
}