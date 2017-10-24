using Model;
using System.Collections.Generic;

namespace Data.Dapper.Interface
{
    public interface ICouponRepository
    {
        void InsertCoupon(string code, int userId);

        int GetCouponCount(int userId);

        List<UserCoupon> GetList(int userId);

        List<UserCoupon> GetList();

        void InsertAwardedCoupon(string code);

        bool CheckAwardedCoupon(string code);

        List<Coupon> GetAwardedCouponList();

        List<UserCoupon> GetAwardedCouponListReport();

        List<UserCoupon> GetCouponListReport();

        bool CheckExistingCoupon(string code);

        bool CheckExistingAwardedCoupon(string code);
    }
}