using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockProject.Entities.Entities;
using System.Data;

namespace StockProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        string baseURL = "https://localhost:7270";

        public async Task<IActionResult> Index()
        {
            List<Order> orders = new List<Order>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Order/GetAllOrder"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    orders = JsonConvert.DeserializeObject<List<Order>>(apiResult);
                }

            }
            return View(orders);

        }
    }
}
