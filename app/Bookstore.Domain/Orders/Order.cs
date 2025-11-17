using Bookstore.Domain.Addresses;
using Bookstore.Domain.Books;
using Bookstore.Domain.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Orders
{
    [Table("Order", Schema = "bobsusedbookstore_dbo")]
    public class Order : Entity
    {
        public Order(int customerId, int addressId)
        {
            CustomerId = customerId;
            AddressId = addressId;
        }

        private readonly List<OrderItem> orderItems = new List<OrderItem>();

        [Column("customerid")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Column("addressid")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public IEnumerable<OrderItem> OrderItems => orderItems;

        [Column("deliverydate")]
        public DateTime DeliveryDate { get; set; } = DateTime.Now.AddDays(7);

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        [Column("tax")]
        public decimal Tax => SubTotal * 0.1m;

        [Column("subtotal")]
        public decimal SubTotal => OrderItems.Sum(x => x.Book.Price);

        [Column("total")]
        public decimal Total => SubTotal + Tax;

        public void AddOrderItem(Book book, int quantity)
        {
            orderItems.Add(new OrderItem(this, book, quantity));
        }
    }
}