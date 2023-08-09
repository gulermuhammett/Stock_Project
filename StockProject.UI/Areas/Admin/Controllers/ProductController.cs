using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockProject.Entities.Entities;
using StockProject.UI.Areas.Admin.Models.DTOs;
using System.Text;

namespace StockProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        string baseURL = "https://localhost:7270";

        public async Task< IActionResult> Index()
        {
            List<Product> products = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Product/GetAllProduct"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(apiResult);
                }

            }
            return View(products);

        }

        
        public async Task<IActionResult> AddProduct()
        {
            List<Category> _categories = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Category/GetAllActive"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    _categories = JsonConvert.DeserializeObject<List<Category>>(apiResult);
                }

            }
            List<StockProject.Entities.Entities.Supplier> _supplier = new List<StockProject.Entities.Entities.Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Supplier/GetAllActive"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    _supplier = JsonConvert.DeserializeObject<List<StockProject.Entities.Entities.Supplier>>(apiResult);
                }

            }
            AddProductDTO productDTO = new AddProductDTO()
            {
                categories = _categories,
                suppliers = _supplier
            };
            return View(productDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDTO productDto)
        {
            Product product = new Product()
            {
                CategoryId = productDto.CategoryId,
                SupplierId = productDto.SupplierId,
                UnitPrice = productDto.product.UnitPrice,
                Stock = productDto.product.Stock,
                ExpireDate = productDto.product.ExpireDate,
                ProductName = productDto.product.ProductName,
                IsActive = true,
            };
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product),Encoding.UTF8,"application/json");
                using (var answ = await httpClient.PostAsync($"{baseURL}/api/Product/CreateProduct",content))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    
                }

            }
            return RedirectToAction("Index");
        }
        static Product _productt;
        static List<Category> _categoriess;
        static List<StockProject.Entities.Entities.Supplier> _supplierr;

        public async Task<IActionResult> UpdateProduct(int id)
        {
          
          
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Product/GetProductById/{id}"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    _productt = JsonConvert.DeserializeObject<Product[]>(apiResult)[0];
                }
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Category/GetAllActive"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    _categoriess = JsonConvert.DeserializeObject<List<Category>>(apiResult);
                }
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Supplier/GetAllActive"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    _supplierr = JsonConvert.DeserializeObject<List<StockProject.Entities.Entities.Supplier>>(apiResult);
                }

            }
            
           
            UpdateProductDTO productDTO = new UpdateProductDTO()
            {
                ProductId= _productt.Id,
                product= _productt,
                categories = _categoriess,
                suppliers = _supplierr
            };

            return View(productDTO);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            Product product = new Product()
            {
                CategoryId = updateProductDTO.CategoryId,
                SupplierId = updateProductDTO.SupplierId,
                UnitPrice = updateProductDTO.product.UnitPrice,
                Stock = updateProductDTO.product.Stock,
                ExpireDate = updateProductDTO.product.ExpireDate,
                ProductName = updateProductDTO.product.ProductName,
                IsActive = updateProductDTO.product.IsActive,
            };
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                using (var answ = await httpClient.PostAsync($"{baseURL}/api/Product/UpdateProduct", content))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();

                }

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{baseURL}/api/Product/DeleteProduct/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

    }
}
