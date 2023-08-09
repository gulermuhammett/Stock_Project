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
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> service;

        public CategoryController(IGenericService<Category> service)
        {
            this.service = service;
        }
        //GET: api/Category/GetAllCategories
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(service.GetAll());
        }
        //GET: api/Category/GetAllActive
        [HttpGet]

        public IActionResult GetAllActive()
        {
            return Ok(service.GetActive());
        }

        //GET: api/Category/GetCategoryById
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            return Ok(service.GetById(id));
        }

        //GET: api/Category/GetCategoryByName
        [HttpGet("{name}")]

        public IActionResult GetCategoryByName(string name)
        {
            return Ok(service.GetDefault(x=>x.CategoryName.ToLower().Contains(name.ToLower())==true).ToList());
        }


        //POST: api/Category/CreateCategory
        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            service.Add(category);

            return CreatedAtAction("GetCategoryById", new { id = category.Id },category);
        }

        //PUT: api/Category/UpdateCategory
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest("Idler eşleşmiyor.Kontrol edip tekrar deneyiniz!");

            try
            {
                service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!service.Any(x => x.Id == id))
                {
                    return NotFound("Böyle bir kategori bulunmamaktadır!");
                }
            }
            return NoContent();

            #region MyRegion
            //else if(!service.Any(x => x.Id == id))
            //{
            //    return NotFound("Böyle bir kategori bulunmamaktadır!");
            //}
            //else
            //{
            //    service.Update(category);
            //    return Ok(category);
            //}
            #endregion



        }

        //Delete: api/Category/DeleteCategory
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = service.GetById(id);
            if (category == null)
                return NotFound();
            try
            {
                service.Remove(category);
                return Ok("Kategori silindi");
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        public IActionResult AvtivateCategory(int id)
        {
            var category = service.GetById(id);
            if (category == null)
                return NotFound();
            //Eğer aktif ise aktifleştirme eklenebilir.
            try
            {
                service.Activate(id);
                return Ok(category);
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }

        }
    }
}
