using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockProject.Entities.Entities;
using System.Security.Policy;
using System.Text;

namespace StockProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        string baseURL = "https://localhost:7270";

        public async Task< IActionResult> Index()
        {
            List<Category> categories = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Category/GetAllCategories"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<Category>>(apiResult);
                }

            }
            return View(categories);
        }

        
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            category.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category),Encoding.UTF8,"application/json");
                using (var answ = await httpClient.PostAsync($"{baseURL}/api/Category/CreateCategory",content))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    
                }

            }
            return RedirectToAction("Index");
        }
        static Category updatedcategory;
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
           

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Category/GetCategoryById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedcategory = JsonConvert.DeserializeObject<Category>(apiCevap);
                }
            }



            return View(updatedcategory);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            
            using (var httpClient = new HttpClient())
            {
                category.IsActive = updatedcategory.IsActive;
                category.AddedDate = updatedcategory.AddedDate;

                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{baseURL}/api/Category/UpdateCategory/{category.Id}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }
        

        public async Task<IActionResult> DeleteCategory(int id)
        {
           
                using (var httpClient = new HttpClient())
                {
                    using (var cevap = await httpClient.DeleteAsync($"{baseURL}/api/Category/DeleteCategory/{id}"))
                    {
                        //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    }
                 }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ActivetedCategory(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Category/AvtivateCategory/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
