using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities.Entities
{
    public class Supplier:BaseEntity
    {
        public Supplier()
        {
            Products = new List<Product>();
        }
        public string SupplierName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        //Navigation Prop

        //Bir tedarikçinin birden fazla ürünü olabilir

        public virtual List<Product> Products { get; set; }
    }
}
