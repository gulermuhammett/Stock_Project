using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockProject.Entities.Entities;
using StockProject.Service.Abstract;
using System.Data;

namespace StockProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> service;

        public ProductController(IGenericService<Product> service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult GetAllProduct()
        {
            var productList = service.GetAll(t0 => t0.Category, t1 => t1.Supplier);
            return Ok(productList);
        }
        [HttpGet("{id}")]
        public ActionResult GetAllProductForSupplier(int id)
        {
            var productList = service.GetAll(x=>x.SupplierId==id, t0 => t0.Category, t1 => t1.Supplier);
            return Ok(productList);
        }
        [HttpGet]
        public ActionResult GetAllActiveProduct()
        {
            return Ok(service.GetActive(t0 => t0.Category, t1 => t1.Supplier));
        }
        [HttpGet("{id}")]
        public ActionResult GetProductById(int id)
        {
            var product=service.GetById(id, t0 => t0.Category, t1 => t1.Supplier);

            return Ok(product);
        }
        //GET: api/Product/GetProductByName
        [HttpGet("{name}")]

        public IActionResult GetProductByName(string name)
        {
            return Ok(service.GetDefault(x => x.ProductName.ToLower().Contains(name.ToLower()) == true).ToList());
        }
        [HttpPost]


        //POST: api/Product/CreateProduct
        public IActionResult CreateProduct([FromBody] Product Product)
        {
            service.Add(Product);

            return CreatedAtAction("GetProductById", new { id = Product.Id }, Product);
        }

        //PUT: api/Product/UpdateProduct
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product Product)
        {
            if (id != Product.Id)
                return BadRequest("Idler eşleşmiyor.Kontrol edip tekrar deneyiniz!");

            try
            {
                service.Update(Product);
                return Ok(Product);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!service.Any(x => x.Id == id))
                {
                    return NotFound("Böyle bir ürün bulunmamaktadır!");
                }
            }
            return NoContent();

            #region MyRegion
            //else if(!service.Any(x => x.Id == id))
            //{
            //    return NotFound("Böyle bir Product bulunmamaktadır!");
            //}
            //else
            //{
            //    service.Update(category);
            //    return Ok(category);
            //}
            #endregion



        }
        //Delete: api/Product/DeleteProduct
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var Product = service.GetById(id);
            if (Product == null)
                return NotFound();
            try
            {
                service.Remove(Product);
                return Ok("Ürün silindi");
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        public IActionResult AvtivateProduct(int id)
        {
            var Product = service.GetById(id);
            if (Product == null)
                return NotFound();
            try
            {
                service.Activate(id);
                return Ok(Product);
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }

        }
    }
}
