using Basket_Kata.Core;
using Basket_Kata.Core.Services;
using Basket_Kata.Core.Services.ProductService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Basket_Kata.Tests
{
    [TestClass]
    public class GiftVoucherTests
    {
        [TestMethod]
        public void Single_Gift_vouchers_redeemed()
        {
            var product1 = new Product { Id = 1, Name = "Jumper", Price = 30M, Category = ProductCategory.Clothing };
            var voucher = new GiftVoucher { Name = "XXX-XXX", Value = 10M };

            var mock = new Mock<IProductService>();
            mock.Setup(s => s.Get(1)).Returns(product1);

            var basket = new Basket(new GiftVoucherService(), mock.Object);
            basket.AddProduct(1);
            basket.AddGiftVoucher(voucher);

            Assert.AreEqual(20.00M, basket.Total);
            Assert.IsNull(basket.Message);
        }

        [TestMethod]
        public void No_Gift_vouchers_redeemed()
        {
            var product1 = new Product { Id = 1, Name = "Jumper", Price = 30M, Category = ProductCategory.Clothing };

            var mock = new Mock<IProductService>();
            mock.Setup(s => s.Get(1)).Returns(product1);

            var basket = new Basket(new GiftVoucherService(), mock.Object);
            basket.AddProduct(1);

            Assert.AreEqual(30.00M, basket.Total);
            Assert.IsNull(basket.Message);
        }

        [TestMethod]
        public void Gift_vouchers_can_only_be_redeemed_against_non_gift_voucher_products()
        {
            var product1 = new Product { Id = 1, Name = "Jumper", Price = 30M, Category = ProductCategory.Clothing };
            var product2 = new Product { Id = 2, Name = "ggg-ggg", Price = 20M, Category = ProductCategory.GiftVoucher };
            var voucher = new GiftVoucher { Name = "XXX-XXX", Value = 50M };

            var mock = new Mock<IProductService>();
            mock.Setup(s => s.Get(1)).Returns(product1);
            mock.Setup(s => s.Get(2)).Returns(product2);

            var basket = new Basket(new GiftVoucherService(), mock.Object);
            basket.AddProduct(1);
            basket.AddProduct(2);
            basket.AddGiftVoucher(voucher);

            Assert.AreEqual(50.00M, basket.Total);
            Assert.AreEqual("Gift vouchers can only be redeemed against non gift voucher products.", basket.Message);
        }

        [TestMethod]
        public void Multiple_Gift_vouchers_can_be_redeemed()
        {
            var product1 = new Product { Id = 1, Name = "Jumper", Price = 30M, Category = ProductCategory.Clothing };
            var product2 = new Product { Id = 2, Name = "Suit", Price = 250M, Category = ProductCategory.Clothing };
            var voucher1 = new GiftVoucher { Name = "XXX-XXX", Value = 50M };
            var voucher2 = new GiftVoucher { Name = "ggg-ggg", Value = 25M };

            var mock = new Mock<IProductService>();
            mock.Setup(s => s.Get(1)).Returns(product1);
            mock.Setup(s => s.Get(2)).Returns(product2);

            var basket = new Basket(new GiftVoucherService(), mock.Object);
            basket.AddProduct(1);
            basket.AddProduct(2);
            basket.AddGiftVoucher(voucher1);
            basket.AddGiftVoucher(voucher2);

            Assert.AreEqual(205M, basket.Total);
            Assert.IsNull(basket.Message);
        }

        [TestMethod]
        public void Voucher_value_is_greater_than_basket_total()
        {
            var product1 = new Product { Id = 1, Name = "Jumper", Price = 30M, Category = ProductCategory.Clothing };
            var voucher1 = new GiftVoucher { Name = "XXX-XXX", Value = 50M };

            var mock = new Mock<IProductService>();
            mock.Setup(s => s.Get(1)).Returns(product1);

            var basket = new Basket(new GiftVoucherService(), mock.Object);
            basket.AddProduct(1);
            basket.AddGiftVoucher(voucher1);

            Assert.AreEqual(0M, basket.Total);
            Assert.AreEqual("You still have �20.00 left on the gift voucher", basket.Message);
        }

        [TestMethod]
        public void Multiple_voucher_total_value_is_greater_than_basket_total()
        {
            var product1 = new Product { Id = 1, Name = "Jumper", Price = 30M, Category = ProductCategory.Clothing };
            var voucher1 = new GiftVoucher { Name = "XXX-XXX", Value = 50M };
            var voucher2 = new GiftVoucher { Name = "XXX-XXX", Value = 50M };

            var mock = new Mock<IProductService>();
            mock.Setup(s => s.Get(1)).Returns(product1);

            var basket = new Basket(new GiftVoucherService(), mock.Object);
            basket.AddProduct(1);
            basket.AddGiftVoucher(voucher1);
            basket.AddGiftVoucher(voucher2);

            Assert.AreEqual(0M, basket.Total);
            Assert.AreEqual("You still have �70.00 left on the gift voucher", basket.Message);
        }
    }
}
