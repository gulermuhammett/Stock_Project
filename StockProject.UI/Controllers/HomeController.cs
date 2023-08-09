using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockProject.Entities.Entities;
using StockProject.UI.Models;
using StockProject.UI.Models.DTOs;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace StockProject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string baseURL = "https://localhost:7270"; //Web API!nın çoluştuğu sunucu portu ile birlikte olacak

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public  IActionResult  Index()
        {
           
            return View();
        }
        public IActionResult LogIn()
        {

            return View();
        }
        [HttpPost]
        //https://localhost:7270/api/User/Login?email=ba%40ba.com&password=12345.A
        public async Task<IActionResult> LogIn(LoginDTO loginVM)
        {
            User logged = new User();
            using (var httpClient=new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/Login?email={loginVM.Email}&password={loginVM.Password}"))
                {
                    string apiResult= await answ.Content.ReadAsStringAsync();
                    logged=JsonConvert.DeserializeObject<User>(apiResult);
                }

            }
            if (logged != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim ("ID",logged.Id.ToString()),
                    new Claim("PhotoUrl",logged.PhotoUrl),
                    new Claim(ClaimTypes.Name,logged.FirstName),
                    new Claim(ClaimTypes.Surname,logged.LastName),
                    new Claim(ClaimTypes.Email,logged.Email),
                    new Claim(ClaimTypes.Role,logged.Role.ToString()),

                };
                var userIdentity = new ClaimsIdentity(claims, "LogIn");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }
            else
            {
                return View(loginVM);
            }
            switch (logged.Role)
            {
                case Entities.Enums.UserRole.Admin:
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                case Entities.Enums.UserRole.Supplier:             
                    return RedirectToAction("Index", "Home", new { Area = "Supplier" });
                case Entities.Enums.UserRole.User:                 
                    return RedirectToAction("Index", "Home", new { Area = "User" });
                default:
                    return View(loginVM);
            }
            
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("LogIn", "Home", new {Area=""});
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}