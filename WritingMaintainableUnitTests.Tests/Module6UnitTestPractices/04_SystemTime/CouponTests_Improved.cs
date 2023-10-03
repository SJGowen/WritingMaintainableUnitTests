using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module6UnitTestPractices.Coupons;

namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices._04_SystemTime
{
    [TestFixture]
    public class When_verifying_whether_a_coupon_is_applicable__improved
    {
        [Test]
        public void It_should_be_applicable_within_one_month_after_the_creation_date_of_the_coupon_V2()
        {
            var coupon = new Coupon("COUPON_CODE", new DateTime(2020, 02, 29));
            var dateWithinValidityPeriod = new DateTime(2020, 03, 31);
            
            Assert.That(coupon.IsApplicableOnV2(dateWithinValidityPeriod));
        }
        
        [Test]
        public void It_should_NOT_be_applicable_more_than_one_month_after_the_creation_date_of_the_coupon_V2()
        {
            var coupon = new Coupon("COUPON_CODE", new DateTime(2020, 02, 29));
            var dateAfterValidityPeriod = new DateTime(2020, 04, 01);
            
            Assert.That(coupon.IsApplicableOnV2(dateAfterValidityPeriod), Is.False);
        }
    }
}