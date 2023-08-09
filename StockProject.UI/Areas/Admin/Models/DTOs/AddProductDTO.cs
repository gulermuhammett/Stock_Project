using StockProject.Entities.Entities;

namespace StockProject.UI.Areas.Admin.Models.DTOs
{
    public class AddProductDTO
    {
        public Product product { get; set; }

       public  List<Category> categories { get;set; }

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

       public List<StockProject.Entities.Entities.Supplier> suppliers { get;set; }

    }
}
