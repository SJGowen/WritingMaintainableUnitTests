using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.Coupons;

public class CreateCouponHandler
{
    private readonly ICouponRepository _couponRepository;
    private readonly IClock _clock;

    public CreateCouponHandler(ICouponRepository couponRepository, IClock clock)
    {
        _couponRepository = couponRepository;
        _clock = clock;
    }

    #region Coupled

    public void Handle_Coupled(CreateCoupon command)
    {
        var coupon = new Coupon(command.CouponCode, DateTime.Today);
        _couponRepository.Save(coupon);
    }

    #endregion

    #region Decoupled

    public void Handle_Decoupled(CreateCoupon command)
    {
        var creationDate = _clock.GetCurrentDate();

        var coupon = new Coupon(command.CouponCode, creationDate);
        _couponRepository.Save(coupon);
    }

    #endregion
}

public class CreateCoupon
{
    public string CouponCode { get; }

    public CreateCoupon(string couponCode)
    {
        CouponCode = couponCode;
    }
}