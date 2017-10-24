using Data.Dapper.Class;
using System;
using Xunit;

namespace UnitTests
{
    public class CouponTest
    {
        [Fact]
        public void GenerateAwardedCoupon()
        {
            // Arrange
            var obj = new CouponRepository();
            Random objRnd = new Random();
            var rnd = objRnd.Next(50000, 1000000).ToString();

            // Act
            obj.InsertAwardedCoupon(rnd);

            // Assert
            Assert.True(obj.CheckExistingAwardedCoupon(rnd));
        }

        [Fact]
        public void GetAwardedCouponList()
        {
            // Arrange
            var obj = new CouponRepository();

            // Act
            var ret = obj.GetAwardedCouponList();

            // Assert
            Assert.True(ret.Count > 0);
        }

        [Fact]
        public void CheckExistingAwardedCoupon()
        {
            // Arrange
            var obj = new CouponRepository();
            Random objRnd = new Random();
            var rnd = objRnd.Next(50000, 1000000).ToString();
            obj.InsertAwardedCoupon(rnd);

            // Act
            var ret = obj.CheckExistingAwardedCoupon(rnd);

            // Assert
            Assert.True(ret);
        }
    }
}