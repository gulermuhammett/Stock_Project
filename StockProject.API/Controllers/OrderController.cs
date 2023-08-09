using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockProject.Entities.Entities;
using StockProject.Entities.Enums;
using StockProject.Service.Abstract;
using System.Data;

namespace StockProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> orderService;
        private readonly IGenericService<OrderDetail> orderDetailService;
        private readonly IGenericService<Product> productService;
        private readonly IGenericService<User> userService;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderDetail> orderDetailService, IGenericService<Product> productService, IGenericService<User> userService)
        {
            this.orderService = orderService;
            this.orderDetailService = orderDetailService;
            this.productService = productService;
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult GetAllOrder()
        {
            var productList = orderService.GetAll(t0 => t0.User, t1 => t1.OrderDetails);
            return Ok(productList);
        }
        [HttpGet("{id}")]
        public ActionResult GetOrderDetails(int id)
        {
            var productList = orderDetailService.GetAll(x => x.OrderId == id, t0 => t0.Product);
            return Ok(productList);
        }
        [HttpGet]
        public ActionResult GetAllActiveOrder()
        {
            return Ok(orderService.GetActive(t0 => t0.OrderDetails, t1 => t1.User));
        }
        [HttpGet("{id}")]
        public ActionResult GetOrderById(int id)
        {
            var product = orderService.GetById(id, t0 => t0.OrderDetails, t1 => t1.User);

            return Ok(product);
        }
        //GET: api/Order/GetPandingOrder
        [HttpGet]
        public IActionResult GetPandingOrder()
        {
            return Ok(orderService.GetDefault(x => x.Status==Status.Pending).ToList());
        }
        [HttpGet]
        //GET: api/Order/GetConfirmedOrder
        public IActionResult GetConfirmedOrder()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Confirmed).ToList());
        }

        //GET: api/Order/GetCanceledOrder
        [HttpGet]
        public IActionResult GetCanceledOrder()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Canceled).ToList());


        }
        //POST: api/Order/DeleteOrder
        [HttpPost]
        public IActionResult CreateOrder([FromQuery] int userId, [FromQuery] int[] productIds, [FromQuery] short[] quantites)
        {

            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.Status = Status.Pending;
            newOrder.IsActive = true;

            orderService.Add(newOrder); //DB'ye 1 satır order eklendi. Ve bu sırada OrderId için kullanılacak ID oluşturuldu

            //int[] productsID = new int { 1, 5, 3, 4 };

            for (int i = 0; i < productIds.Length; i++)
            {
                OrderDetail detail = new OrderDetail();
                detail.OrderId= newOrder.Id;
                detail.ProductId= productIds[i];
                detail.Quantity= quantites[i];
                detail.UnitPrice = productService.GetById(productIds[i]).UnitPrice;
                detail.IsActive = true;

                orderDetailService.Add(detail);
            }

            return CreatedAtAction("GetOrderById", new { id = newOrder.Id }, newOrder);
        }

        [HttpGet("{id}")]
        public IActionResult ConfirmedToOrder(int id)
        {
            Order order = orderService.GetById(id);
            if(order==null)
                return NotFound("Sipariş bulunamadı!");
            else
            {
                List<OrderDetail> detaylar = orderDetailService.GetDefault(x => x.OrderId == id).ToList();
                foreach (OrderDetail item in detaylar)
                {
                    Product detaydaki = productService.GetById(item.ProductId);
                    detaydaki.Stock -= item.Quantity;
                    productService.Update(detaydaki);

                    item.IsActive = false;
                    orderDetailService.Update(item);
                }
                order.Status = Status.Confirmed;
                order.IsActive = false;
                orderService.Update(order);

                return Ok("Siparişiniz onaylandı");
            }

        }
        [HttpGet("{id}")]
        public IActionResult CanceledToOrder(int id)
        {
            Order order = orderService.GetById(id);
            if (order == null)
                return NotFound("Sipariş bulunamadı!");
            else
            {
                List<OrderDetail> detaylar = orderDetailService.GetDefault(x => x.OrderId == id).ToList();
                foreach (OrderDetail item in detaylar)
                {
                    

                    item.IsActive = false;
                    orderDetailService.Update(item);
                }
                order.Status = Status.Canceled;
                order.IsActive = false;
                orderService.Update(order);

                return Ok("Siparişiniz İPTAL EDİLDİ");
            }

        }


    }
}
