using CShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Data.ViewModel
{
        public class ListOrdersViewModel
        {
            public int OrderId { set; get; }
            public DateTime OrderDate { set; get; }
            public Guid UserId { set; get; }
            public string ShipName { set; get; }
            public string ShipAddress { set; get; }
            public string ShipEmail { set; get; }
            public string ShipPhoneNumber { set; get; }
            public int OrderStatus { set; get; }
            public IEnumerable<OrderDetailViewModel> Detail { set; get; }
        }

        public class OrderDetailViewModel
        {
            public int Id { set; get; }
            public int OrderId { set; get; }
            public int ProductId { set; get; }
            public int Quantity { set; get; }
            public decimal Price { set; get; }
            public Product? Product { get; set; }
        }

}
