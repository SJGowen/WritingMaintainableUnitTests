using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Tests.Common;

namespace WritingMaintainableUnitTests.Tests.Module5AssertionsAndObservations._01_ObservationSyntax
{
    namespace ClassicAssertionModel
    {
        [TestFixture]
        public class WrongUse 
        {
            [Test]
            public void Some_result_should_be_observed()
            {
                var sum = 1 + 1;
                Assert.AreEqual(sum, 2);
            }
        }
    
        [TestFixture]
        public class CorrectUse 
        {
            [Test]
            public void Some_result_should_be_observed()
            {
                var sum = 1 + 1;
                Assert.AreEqual(2, sum);
            }
        }    
    }

    namespace ConstraintAssertionModel
    {
        [TestFixture]
        public class ConstraintModel 
        {
            [Test]
            public void Some_result_should_be_observed()
            {
                var sum = 1 + 1;
                Assert.That(sum, Is.EqualTo(2));
            }
        }         
    }

    namespace CustomObservationMethods
    {
        [Specification]
        public class When_testing_something 
        {
            [Observation]
            public void Then_some_result_should_be_observed()
            {
                var sum = 1 + 1;
                sum.Should_be_equal_to(2);
            }
        }    
    }

    namespace AssertionMessages
    {
        [TestFixture]
        public class MessageTests
        {
            [Test]
            public void Some_result_should_be_observed()
            {
                var sum = 1 + 1;
                
                Assert.That(sum, Is.EqualTo(2), 
                    "The sum of 1 and 1 should be 2");
            }    
        }
    }

    namespace ExceptionObservations
    {   
        [TestFixture]
        public class ExceptionTests
        {            
            [Test]
            public void An_exception_should_be_thrown()
            {
                TestDelegate broken = () => throw new InvalidOperationException("Explosion!");
                Assert.That(broken, Throws.TypeOf<InvalidOperationException>());
            }
        }
        
        [Specification]
        public class When_testing_an_exceptional_situation
        {
            [Observation]
            public void Then_an_exception_should_be_thrown()
            {
                Action broken = () => throw new InvalidOperationException("Explosion!");
                
                var caughtException = broken.Should_throw_an<InvalidOperationException>();
                caughtException.Message.Should_be_equal_to("Explosion!");
            }    
        }
    }
}