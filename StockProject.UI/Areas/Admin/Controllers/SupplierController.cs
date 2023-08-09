using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockProject.Entities.Entities;
using System.Data;
using System.Text;

namespace StockProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class SupplierController : Controller
    {
       
        string baseURL = "https://localhost:7270";

        public async Task<IActionResult> Index()
        {
            List<Supplier> supplier = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Supplier/GetAllSuppliers"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    supplier = JsonConvert.DeserializeObject<List<Supplier>>(apiResult);
                }

            }
            return View(supplier);
        }


        public IActionResult AddSupplier()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            supplier.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");
                using (var answ = await httpClient.PostAsync($"{baseURL}/api/Supplier/CreateSupplier", content))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();

                }

            }
            return RedirectToAction("Index");
        }
        static Supplier updatedsupplier;
        [HttpGet]
        public async Task<IActionResult> UpdateSupplier(int id)
        {


            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Supplier/GetSupplierById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedsupplier = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                }
            }



            return View(updatedsupplier);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSupplier(Supplier supplier)
        {

            using (var httpClient = new HttpClient())
            {
                supplier.IsActive = updatedsupplier.IsActive;
                supplier.AddedDate = updatedsupplier.AddedDate;

                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{baseURL}/api/Supplier/UpdateSupplier/{supplier.Id}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteSupplier(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{baseURL}/api/Supplier/DeleteSupplier/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ActivetedSupplier(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Supplier/AvtivateSupplier/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
