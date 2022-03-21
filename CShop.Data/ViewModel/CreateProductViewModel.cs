using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Data.ViewModel
{
    public class CreateProductViewModel
    {
        public string Name { set; get; }
        public IFormFile? UrlImage { set; get; }
        public string? Description { set; get; }

        public decimal Price { set; get; }
        public decimal? OriginalPrice { set; get; }

        public string Details { set; get; }
        public int Quantity { set; get; }

        public int Stock { set; get; }


        public int? CategoryId { set; get; }
    }
}
