using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManagerWeb
{
    public class OrderService : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        private List<Order> OrderList;
        //private Dictionary<string, string> CustomerDict;
        private int OrderNum;
        public int OrderCount { get => OrderList.Count(); }
        public OrderService()
        {
            OrderNum = 0;
            OrderList = new List<Order>();
        }
        public bool AddOrder(Customer customer)
        {
            try
            {
                OrderNum += 1;
                OrderList.Add(new Order(OrderNum.ToString().PadLeft(8, '0'), customer));
            }
            catch { return false; }
            return true;
        }
        public bool AddOrderItem(string OrderNo, OrderItem item)
        {
            int index = OrderList.FindIndex(o => o.OrderNo == OrderNo);
            if (index == -1)
            {
                return false;
            }
            OrderList[index].items.Add(item);
            return true;
        }
        [HttpDelete]
        public bool DeleteOrder(string OrderNo)
        {
            int index = OrderList.FindIndex(o => o.OrderNo == OrderNo);
            if (index == -1)
            {
                return false;
            }
            OrderList.RemoveAt(index);
            return true;
        }
        [HttpGet]
        public IEnumerable<Order> QueryOrder(string query = "", string _type = "")
        {
            IEnumerable<Order> result = OrderList;
            switch (_type.ToLower())
            {
                case "orderno":
                    result = OrderList.Where(o => o.OrderNo == query);
                    break;
                case "productname":
                    result = OrderList.Where(o => o.items.Where(i => i.product.Name == query).Any());
                    break;
                case "custormername":
                    result = OrderList.Where(o => o.customer.Name == query);
                    break;
            }
            return result.OrderByDescending(r => r.TotalPrice);
        }
        public List<Order> SortOrder(string by = "ascending")
        {
            if (by == "ascending")
            {
                return SortOrder((o1, o2) =>
                {
                    if (int.Parse(o1.OrderNo) < int.Parse(o2.OrderNo)) return 1;
                    else if (int.Parse(o1.OrderNo) == int.Parse(o2.OrderNo)) return 0;
                    else return -1;
                });
            }
            else
            {
                return SortOrder((o1, o2) =>
                {
                    if (int.Parse(o1.OrderNo) > int.Parse(o2.OrderNo)) return 1;
                    else if (int.Parse(o1.OrderNo) == int.Parse(o2.OrderNo)) return 0;
                    else return -1;
                });
            }
        }
        public List<Order> SortOrder(Comparison<Order> comparison)
        {
            OrderList.Sort(comparison);
            return OrderList;
        }
        public void Export(string filename = "export.xml")
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = string.IsNullOrWhiteSpace(filename) ?
                    new System.Xml.Serialization.XmlSerializer(OrderList.GetType()) :
                    new System.Xml.Serialization.XmlSerializer(OrderList.GetType(), new XmlRootAttribute("Order"));
                    xmlSerializer.Serialize(writer, OrderList);
                }
                finally
                {
                    writer.Close();
                }
            }
        }
        public void Import(string filename = "export.xml")
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(OrderList.GetType(), new XmlRootAttribute("Order"));
                    OrderList = (List<Order>)xmlSerializer.Deserialize(reader);
                }
                finally
                {
                    reader.Close();
                }
            }

        }
    }

    [XmlRootAttribute("Order")]
    public class Order
    {
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
            get
            {
                int sum = 0;
                items.ForEach(i => sum += i.product.Price * i.Count);
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
            string table = "下单时间：" + OrderTime + _customer.ToString() + "\n";
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


    public class OrderItem
    {
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
        public OrderItem(Product prod, int count)
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

    public class Product
    {
        private string _name;
        private string _type;
        private int _price;
        public string Name
        {
            get => _name; set => _name = value;
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
        public Product(string name, string type, int price)
        {
            Name = name;
            Type = type;
            Price = price;
        }
        public override string ToString()
        {
            return "产品名：" + Name + " 产品类型：" + Type + " 产品价格：" + Price;
        }
    }

    public class Customer
    {
        private string _name;
        private string _phonenum;

        public string Name
        {
            get => _name; set => _name = value;
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
