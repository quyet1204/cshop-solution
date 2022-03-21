using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Data.ViewModel
{
    public class ListCartViewModel
    {
        public int ProductId { set; get; }
        public string ProductSlug { set; get; }
        public string ProductImage { set; get; }
        public string ProductName { set; get; }
        public decimal? CartPrice { set; get; }
        public int CartQuantity { set; get; }
        public int CartTotal { set; get; }
        public decimal? CartId { set; get; }
    }
}
