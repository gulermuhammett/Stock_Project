using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities.Entities
{
    public class Product:BaseEntity
    {
        public Product()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public short? Stock { get; set; }

        public DateTime? ExpireDate { get; set; }

        //Navigation Properties

        //Bir ürünün bir kategorisi olur
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        //Bir ürünün bir tedarikçisi olur
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        //Bir ürün birden fazla sipariş datayının içerisinde olabilir.
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
