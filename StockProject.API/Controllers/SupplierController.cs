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
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> service;

        public SupplierController(IGenericService<Supplier> service)
        {
            this.service = service;
        }
        //GET: api/Supplier/GetAllSuppliers
        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            return Ok(service.GetAll());
        }
        //GET: api/Supplier/GetAllActive
        [HttpGet]

        public IActionResult GetAllActive()
        {
            return Ok(service.GetActive());
        }

        //GET: api/Supplier/GetSupplierById
        [HttpGet("{id}")]

        public IActionResult GetSupplierById(int id)
        {
            return Ok(service.GetById(id));
        }

        //GET: api/Supplier/GetSupplierByName
        [HttpGet("{name}")]

        public IActionResult GetSupplierByName(string name)
        {
            return Ok(service.GetDefault(x=>x.SupplierName.ToLower().Contains(name.ToLower())==true).ToList());
        }
        [HttpPost]

        //POST: api/Supplier/CreateSupplier
        public IActionResult CreateSupplier([FromBody] Supplier supplier)
        {
            service.Add(supplier);

            return CreatedAtAction("GetSupplierById", new { id = supplier.Id }, supplier);
        }

        //PUT: api/Supplier/UpdateSupplier
        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, [FromBody] Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest("Idler eşleşmiyor.Kontrol edip tekrar deneyiniz!");

            try
            {
                service.Update(supplier);
                return Ok(supplier);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!service.Any(x => x.Id == id))
                {
                    return NotFound("Böyle bir supplier bulunmamaktadır!");
                }
            }
            return NoContent();

            #region MyRegion
            //else if(!service.Any(x => x.Id == id))
            //{
            //    return NotFound("Böyle bir supplier bulunmamaktadır!");
            //}
            //else
            //{
            //    service.Update(category);
            //    return Ok(category);
            //}
            #endregion



        }
        //Delete: api/Supplier/DeleteSupplier
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = service.GetById(id);
            if (supplier == null)
                return NotFound();
            try
            {
                service.Remove(supplier);
                return Ok("Kategori silindi");
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        public IActionResult AvtivateSupplier(int id)
        {
            var supplier = service.GetById(id);
            if (supplier == null)
                return NotFound();
            try
            {
                service.Activate(id);
                return Ok(supplier);
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }

        }
    }
}
