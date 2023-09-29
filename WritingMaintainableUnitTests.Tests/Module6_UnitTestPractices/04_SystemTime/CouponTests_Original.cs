using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module6_UnitTestPractices.Coupons;

// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices._04_SystemTime
{
    [TestFixture]
    public class When_verifying_whether_a_coupon_is_applicable__original
    {
        // TODO: Good use-case for property-based testing??
        [Test]
        public void Verification_of_all_dates_in_a_single_year()
        {
            var currentDate = new DateTime(2020, 01, 01);
            while(currentDate < new DateTime(2020, 12, 31))
            {
                var oneMonthFromToday = currentDate.AddMonths(1);
                var thirtyDaysFromToday = currentDate.AddDays(30);
                if(! (thirtyDaysFromToday <= oneMonthFromToday))
                {
                    Console.WriteLine(currentDate);
                }

                currentDate = currentDate.AddDays(1);
            }
        }
        
        [Test]
        public void It_should_be_applicable_within_one_month_after_the_creation_date_of_the_coupon()
        {
            var coupon = new Coupon("COUPON_CODE", DateTime.Today);
            var thirtyDaysFromToday = DateTime.Today.AddDays(30);
         
            // In 2020, this test fails from 31 Jan until 29 Feb
            Assert.That(coupon.IsApplicableOn(thirtyDaysFromToday));
        }
        
        [Test]
        public void It_should_NOT_be_applicable_more_than_one_month_after_the_creation_date_of_the_coupon()
        {
            var coupon = new Coupon("COUPON_CODE", DateTime.Today);
            var thirtyDaysFromToday = DateTime.Today.AddDays(32);
            
            Assert.That(coupon.IsApplicableOn(thirtyDaysFromToday), Is.False);
        }
    }
}