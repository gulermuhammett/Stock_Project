using StockProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities.Entities
{
    public class User:BaseEntity
    {
        public User()
        {
            Orders = new List<Order>();
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? PhotoUrl { get; set; }

        public string Email { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        //NP
        //Bir kullanıcının birden fazla siparişi olabilir
        public virtual List<Order> Orders { get; set; }
    }
}
