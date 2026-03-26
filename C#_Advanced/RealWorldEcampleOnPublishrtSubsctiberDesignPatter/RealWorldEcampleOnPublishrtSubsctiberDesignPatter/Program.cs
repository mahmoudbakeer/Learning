using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldEcampleOnPublishrtSubsctiberDesignPatter
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Order order = new Order();
            ShipingService shipingService = new ShipingService();
            EmailingService emailingService = new EmailingService();
            SmsService smsService = new SmsService();



            // subscribtion
            shipingService.Subscribe(order);
            emailingService.Subscribe(order);
            smsService.Subscribe(order);


            // UnSubscribtion
            //shipingService.Unsubscribe(order);
            //emailingService.Unsubscribe(order);
            //smsService.Unsubscribe(order);


            // publishing
            order.OrderCreate(1, 100, "mahmoudbakir@gmail.com");

        }
    }



    public class OrderEventArgs : EventArgs
    {
        public int OrderID { get; }
        public decimal TotalPrice { get; }
        public string Email { get;  }

        public OrderEventArgs(int OrderID , decimal TotalPrice , string Email)
        {
            this.OrderID = OrderID;
            this.TotalPrice = TotalPrice;
            this.Email = Email;
        }
    }
    public class Order
    {
        public EventHandler<OrderEventArgs> OrderChanged;
        public void OrderCreate(int OrderID, decimal TotalPrice, string Email)

        {
            Console.WriteLine("The order paced and the publishing to subscriber begins........");
            OrderChanged?.Invoke(this , new OrderEventArgs(OrderID, TotalPrice, Email));
        }
    }

    public class ShipingService
    {


        public void Subscribe(Order order)
        {
            order.OrderChanged += HandleShipingService;
        }
        public void Unsubscribe(Order order)
        {
            order.OrderChanged -= HandleShipingService;
        }
        public void HandleShipingService(object sender, OrderEventArgs e)
        {
            Console.WriteLine();

            Console.WriteLine("..............ShipingService...............");
            Console.WriteLine($"the order id is : {e.OrderID}");
            Console.WriteLine($"the receiver email is : {e.Email}");
            Console.WriteLine($"the Total Price of order is : {e.TotalPrice}");
            Console.WriteLine("...........................................");
            Console.WriteLine();

        }
    }
    public class EmailingService
    {
        public void Subscribe(Order order)
        {
            order.OrderChanged += HandleEmailingService;
        }
        public void Unsubscribe(Order order)
        {
            order.OrderChanged -= HandleEmailingService;
        }
        public void HandleEmailingService(object sender, OrderEventArgs e)
        {
            Console.WriteLine();

            Console.WriteLine("..............EmailingService..............");
            Console.WriteLine($"the order id is : {e.OrderID}");
            Console.WriteLine($"the receiver email is : {e.Email}");
            Console.WriteLine($"the Total Price of order is : {e.TotalPrice}");
            Console.WriteLine("...........................................");
            Console.WriteLine();

        }
    }
    public class SmsService
    {
        public void Subscribe(Order order)
        {
            order.OrderChanged += HandleSmsService;
        }
        public void Unsubscribe(Order order)
        {
            order.OrderChanged -= HandleSmsService;
        }
        public void HandleSmsService(object sender, OrderEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("..............SmSSerivce...................");
            Console.WriteLine($"the order id is : {e.OrderID}");
            Console.WriteLine($"the receiver email is : {e.Email}");
            Console.WriteLine($"the Total Price of order is : {e.TotalPrice}");
            Console.WriteLine("...........................................");
            Console.WriteLine();

        }
    }

}
