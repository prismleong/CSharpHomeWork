using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManager;

namespace OrderUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        OrderService service = new OrderService();
        Product phone_xm = new Product("xiaomi", "phone", 2000);
        Product phone_hw = new Product("huawei", "phone", 4000);
        Product pc_dell = new Product("dell", "pc", 6000);
        Customer cus1 = new Customer("Leijun", "13866666666");
        Customer cus2 = new Customer("lzt", "15666666666");

        OrderItem item_phone_xm, item_phone_hw, item_pc_dell, item_phone_xm2, item_phone_hw2;

        [TestInitialize()]
        public void init()
        {
            item_phone_xm = new OrderItem(phone_xm, 10);
            item_phone_hw = new OrderItem(phone_hw, 5);
            item_pc_dell = new OrderItem(pc_dell, 3);
            item_phone_xm2 = new OrderItem(phone_xm, 8);
            item_phone_hw2 = new OrderItem(phone_hw, 1);
        }
        [TestMethod]
        public void AddOrderTest()
        {
            service.AddOrder(cus1);
            service.AddOrderItem("00000001", item_phone_xm);
            service.AddOrderItem("00000001", item_phone_hw);
            service.AddOrderItem("00000001", item_pc_dell);
            service.AddOrder(cus2);
            service.AddOrderItem("00000002", item_phone_xm2);
            service.AddOrderItem("00000002", item_phone_hw2);
            Assert.AreEqual(2, service.OrderCount);
        }

    }
}
