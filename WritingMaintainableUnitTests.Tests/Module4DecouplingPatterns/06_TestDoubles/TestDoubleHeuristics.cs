using System;
using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._06_TestDoubles
{
    [TestFixture]
    public class TestDoubleHeuristics
    {
        [Test]
        public void AvoidTestDoublesReturningTestDoubles()
        {
            var approver = Substitute.For<ICanApproveExpenses>();
            var approverRepository = Substitute.For<IApproverRepository>();

            // Return a real instance instead
            approverRepository.Get(Arg.Any<Guid>()).Returns(approver);
        }
        
        [Test]
        public void AvoidImplementingBehaviourInTestDoubles()
        {
            HeadOfDepartment headOfDepartment = Example.HeadOfDepartment()
                .WithId(new Guid("224FE5B8-EDBB-4F8B-8654-715C1C294CFD"));
            
            var approverRepository = Substitute.For<IApproverRepository>();
            approverRepository.Get(Arg.Any<Guid>()).Returns( callInfo =>
            {
                var id = callInfo.Arg<Guid>();
                return id != Guid.Empty ? headOfDepartment : null;
            });
        }
    }
}