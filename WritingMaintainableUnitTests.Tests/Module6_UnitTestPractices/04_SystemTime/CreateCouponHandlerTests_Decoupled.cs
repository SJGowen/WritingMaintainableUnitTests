using System;
using NSubstitute;
using WritingMaintainableUnitTests.Module6_UnitTestPractices.Coupons;
using WritingMaintainableUnitTests.Tests.Common;

// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices._04_SystemTime
{
    [Specification]
    public class When_creating_a_new_coupon__decoupled
    {
        [Establish]
        public void Context()
        {
            _couponRepository = Substitute.For<ICouponRepository>();

            var clock = Substitute.For<IClock>();
            clock.GetCurrentDate().Returns(new DateTime(2020, 08, 01));
            
            _sut = new CreateCouponHandler(_couponRepository, clock);
        }

        [Because]
        public void Of()
        {
            var command = new CreateCoupon("COUPON_CODE");
            _sut.Handle_Decoupled(command);
        }
        
        [Observation]
        public void Then_a_newly_created_coupon_should_be_saved()
        {
            var expectedCouponToBeSaved = new Coupon("COUPON_CODE", new DateTime(2020, 08, 01));
            _couponRepository.Should_have_received(expectedCouponToBeSaved);        
        }

        private ICouponRepository _couponRepository;
        private CreateCouponHandler _sut;
    }
}