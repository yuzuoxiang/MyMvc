using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMvc.Models;
using System.Linq;
using Moq;

namespace MyMvc.测试
{
    [TestClass]
    public class UnitTest2
    {
        private Product[] products ={
            new Product{ Name="aaa",Category="wkwekee",Price=25M},
            new Product{ Name="bbb",Category="sdfsdfd",Price=13M},
            new Product{ Name="ccc",Category="ewewewr",Price=52M},
            new Product{ Name="ddd",Category="ggdsdfs",Price=44M},
            new Product{ Name="eee",Category="aasdddd",Price=66M}
        };

        [TestMethod]
        public void Sum_Products_Correctly()
        {
            //准备
            //创建模仿对象
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            //选择方法
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
            .Returns<decimal>(total => total);
            var target = new LinqValueCalculator(mock.Object);

            //动作
            var result = target.ValueProducts(products);
            //断言
            Assert.AreEqual(products.Sum(e => e.Price), result);
        }

        private Product[] createProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Pass_Through_Variable_Discounts()
        {
            //准备
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0)))
                .Throws<System.ArgumentOutOfRangeException>();
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100)))
                .Returns<decimal>(total => total * 0.9m);
            mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100, Range.Inclusive)))
                .Returns<decimal>(total => total - 5);

            var target = new LinqValueCalculator(mock.Object);
            //动作
            decimal FiveDollarDiscount = target.ValueProducts(createProduct(5));
            decimal TenDollarDiscount = target.ValueProducts(createProduct(10));
            decimal FiftyDollarDiscount = target.ValueProducts(createProduct(50));
            decimal HundredDollarDiscount = target.ValueProducts(createProduct(100));
            decimal FiveHundredDollarDiscount = target.ValueProducts(createProduct(500));
            //断言
            Assert.AreEqual(5, FiveDollarDiscount, "$5 Fail");
            Assert.AreEqual(10, FiveDollarDiscount, "$10 Fail");
            Assert.AreEqual(45, FiveDollarDiscount, "$50 Fail");
            Assert.AreEqual(95, FiveDollarDiscount, "$100 Fail");
            Assert.AreEqual(450, FiveDollarDiscount, "$500 Fail");
        }
    }
}
