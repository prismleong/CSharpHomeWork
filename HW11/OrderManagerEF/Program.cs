using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OrderManager
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            
            
            
            foreach (var i in service.QueryOrder())
            {
                Console.WriteLine(i);
            }
            service.Export();
            service.Import();
            foreach (var i in service.QueryOrder())
            {
                Console.WriteLine(i);
            }*/

        }
    }

    [XmlRootAttribute("Order")]
    public class Order {
        public string Id;
        public string OrderNo;//订单号
        public enum State { Unfinished, Finished }
        public State OrderState;//订单状态
        private DateTime _dt;
        public DateTime OrderTime
        {
            get => _dt;
            set => _dt = value;
        }
        public List<OrderItem> items;
        private Customer _customer;
        public Customer customer { get => _customer; set => _customer = value; }
        public int TotalPrice
        {
            get { 
                int sum = 0;
                items.ForEach(i => sum += i.product.Price*i.Count);
                return sum; 
            }
        }
        public int OrderItemCount
        {
            get => items.Count();
        }
        public Order() { }
        public Order(string No, Customer p_customer)
        {
            OrderNo = No;
            items = new List<OrderItem>();
            OrderTime = DateTime.Now;
            OrderState = State.Unfinished;
            customer = p_customer;
        }
        public override string ToString()
        {
            string table = "下单时间："+ OrderTime+_customer.ToString()+"\n";
            items.ForEach(i => table += i.ToString() + "\n");
            return table;
        }
        public bool Equals(Order other)
        {
            if (ReferenceEquals(null, other)) return false;

            if (ReferenceEquals(this, other)) return true;

            if (!string.Equals(this.ToString(), other.ToString(), StringComparison.InvariantCulture))
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != this.GetType()) return false;

            return Equals((Order)obj);
        }
        public override int GetHashCode()
        {
            return (this.ToString() != null ? StringComparer.InvariantCulture.GetHashCode(this.ToString()) : 0);
        }

        public static bool operator ==(Order left, Order right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Order left, Order right)
        {
            return !Equals(left, right);
        }
    }


    public class OrderItem {
        private Product _product;
        public Product product { get => _product; set => _product = value; }
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                if (value <= 0) _count = 1;
                else _count = value;
            }
        }
        public OrderItem() { }
        public OrderItem(Product prod,int count)
        {
            product = prod;
            Count = count;
        }
        public override string ToString()
        {
            return product + " 产品件数：" + Count;
        }
        public bool Equals(OrderItem other)
        {
            //this非空，obj如果为空，则返回false
            if (ReferenceEquals(null, other)) return false;

            //如果为同一对象，必然相等
            if (ReferenceEquals(this, other)) return true;

            //对比各个字段值
            if (!string.Equals(this.ToString(), other.ToString(), StringComparison.InvariantCulture))
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            //this非空，obj如果为空，则返回false
            if (ReferenceEquals(null, obj)) return false;

            //如果为同一对象，必然相等
            if (ReferenceEquals(this, obj)) return true;

            //如果类型不同，则必然不相等
            if (obj.GetType() != this.GetType()) return false;

            //调用强类型对比
            return Equals((OrderItem)obj);
        }
        public override int GetHashCode()
        {
            return (this.ToString() != null ? StringComparer.InvariantCulture.GetHashCode(this.ToString()) : 0);
        }

        public static bool operator ==(OrderItem left, OrderItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OrderItem left, OrderItem right)
        {
            return !Equals(left, right);
        }
    }
    public class OrderService
    {

        public OrderService()
        {
        }

        public static List<Order> GetAllOrders()
        {
            using (var db = new OrderContext())
            {
                return AllOrders(db).ToList();
            }
        }

        public static Order GetOrder(string id)
        {
            using (var db = new OrderContext())
            {
                return AllOrders(db).FirstOrDefault(o => o.Id == id);
            }
        }

        public static Order AddOrder(Order order)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                return order;
            }
            catch (Exception e)
            {
                //TODO 需要更加错误类型返回不同错误信息
                throw new ApplicationException($"添加错误: {e.Message}");
            }
        }

        public static void RemoveOrder(string id)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var order = db.Orders.Include("Items").Where(o => o.Id == id).FirstOrDefault();
                    db.Orders.Remove(order);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //TODO 需要更加错误类型返回不同错误信息
                throw new ApplicationException($"删除订单错误!");
            }
        }

        public static void UpdateOrder(Order newOrder)
        {
            RemoveItems(newOrder.Id);
            using (var db = new OrderContext())
            {
                db.Entry(newOrder).State = System.Data.Entity.EntityState.Modified;
                db.OrderItems.AddRange(newOrder.items);
                db.SaveChanges();
            }
        }

        private static void RemoveItems(string orderId)
        {
            using (var db = new OrderContext())
            {
                var oldItems = db.OrderItems.Where(item => item.OrderId == orderId);
                db.OrderItems.RemoveRange(oldItems);
                db.SaveChanges();
            }
        }

        public static List<Order> QueryOrdersByGoodsName(string goodsName)
        {
            using (var db = new OrderContext())
            {
                var query = AllOrders(db)
                  .Where(o => o.items.Count(i => i.product.Name == goodsName) > 0);
                return query.ToList();
            }
        }

        public static List<Order> QueryOrdersByCustomerName(string customerName)
        {
            using (var db = new OrderContext())
            {
                var query = AllOrders(db)
                  .Where(o => o.customer.Name == customerName);
                return query.ToList();
            }
        }

        public static object QueryByTotalAmount(float amout)
        {
            using (var db = new OrderContext())
            {
                return AllOrders(db)
                  .Where(o => o.items.Sum(item => item.product.Price * item.Count) > amout)
                  .ToList();
            }
        }

        private static IQueryable<Order> AllOrders(OrderContext db)
        {
            return db.Orders.Include(o => o.items.Select(i => i.product))
                      .Include("Customer");
        }

        public static void Export(String fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xs.Serialize(fs, GetAllOrders());
            }
        }

        public static void Import(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                List<Order> temp = (List<Order>)xs.Deserialize(fs);
                temp.ForEach(order => {
                    try
                    {
                        AddOrder(order);
                    }
                    catch
                    {
                        //ignore errors
                    }
                });
            }
        }




    }

    public class Product {
        private string _name;
        private string _type;
        private int _price;
        public string Name
        {
            get => _name;set => _name = value;
        }
        public string Type
        {
            get => _type; set => _type = value;
        }
        public int Price
        {
            get => _price; set => _price = value;
        }
        public Product() { }
        public Product(string name,string type, int price)
        {
            Name = name;
            Type = type;
            Price = price;
        }
        public override string ToString()
        {
            return "产品名：" + Name + " 产品类型：" + Type + " 产品价格："+Price;
        }
    }

    public class Customer {
        private string _name;
        private string _phonenum;

        public string Name
        {
            get => _name;set => _name = value;
        }

        public string PhoneNum
        {
            get => _phonenum; set => _phonenum = value;
        }
        public Customer() { }
        public Customer(string name, string phonenum)
        {
            Name = name;
            PhoneNum = phonenum;
        }
        public override string ToString()
        {
            return "客户名：" + Name + " 电话号码：" + PhoneNum;
        }
    }
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string message) : base(message)
        {
            Console.WriteLine("找不到订单号：" + message);
        }
    }

}
