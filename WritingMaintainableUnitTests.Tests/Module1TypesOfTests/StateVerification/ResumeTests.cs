namespace WritingMaintainableUnitTests.Tests.Module1TypesOfTests.StateVerification;

using System;
using System.Linq;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module1TypesOfTests.StateVerification;

[TestFixture]
public class When_adding_experience_to_a_resume
{
    [Test]
    public void Then_the_specified_experience_should_now_appear_on_the_resume()
    {
        var experienceFrom = new DateTime(2014, 09, 12);
        var experienceUntil = new DateTime(2017, 12, 31);

        var resume = new Resume();
        resume.AddExperience("Google", "Data analyst", experienceFrom, experienceUntil);

        var addedExperience = resume.Experiences.SingleOrDefault();
        Assert.That(addedExperience, Is.Not.Null);
        Assert.That(addedExperience.Employer, Is.EqualTo("Google"));
        Assert.That(addedExperience.Role, Is.EqualTo("Data analyst"));
        Assert.That(addedExperience.From, Is.EqualTo(experienceFrom));
        Assert.That(addedExperience.Until, Is.EqualTo(experienceUntil));
    }
}