using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities.Entities
{
    public class BaseEntity
    {
        [Column(Order =1)] //Bütün oluşturulacak entitylerde bu kolan 1. sırada olacak şekilde ayarlandı

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
