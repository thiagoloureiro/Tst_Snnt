using Data.Dapper.Interface;
using Model;
using Service.Interface;
using System.Collections.Generic;
using Utils.Exceptions;

namespace Service.Class
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public bool CheckExistingCoupon(string code)
        {
            return _couponRepository.CheckExistingCoupon(code);
        }

        public bool CheckExistingAwardedCoupon(string code)
        {
            return _couponRepository.CheckExistingAwardedCoupon(code);
        }

        public bool InsertCoupon(string code, int userId)
        {
            if (CheckExistingCoupon(code))
                throw new CouponDuplicatedException();

            if (GetCouponCount(userId) < 5)
            {
                _couponRepository.InsertCoupon(code, userId);
                return CheckAwardedCoupon(code);
            }
            throw new CouponExceededException();
        }

        public int GetCouponCount(int userId)
        {
            return _couponRepository.GetCouponCount(userId);
        }

        public List<UserCoupon> GetList(int userId)
        {
            return _couponRepository.GetList(userId);
        }

        public List<UserCoupon> GetList()
        {
            return _couponRepository.GetList();
        }

        public void InsertAwardedCoupon(string code)
        {
            if (CheckExistingAwardedCoupon(code))
                throw new CouponDuplicatedException();

            _couponRepository.InsertAwardedCoupon(code);
        }

        public bool CheckAwardedCoupon(string code)
        {
            return _couponRepository.CheckAwardedCoupon(code);
        }

        public List<Coupon> GetAwardedCouponList()
        {
            return _couponRepository.GetAwardedCouponList();
        }

        public List<UserCoupon> GetAwardedCouponListReport()
        {
            return _couponRepository.GetAwardedCouponListReport();
        }
    }
}