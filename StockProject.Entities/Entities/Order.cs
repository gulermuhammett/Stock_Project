using StockProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities.Entities
{
    public class Order:BaseEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        //NP
        //Bir siparişi sadece bir kullanıcı verebilir
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public Status Status { get; set; }

        //Navigation Properties

        //Bir siparişte birden fazla order detayı olabilir
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
