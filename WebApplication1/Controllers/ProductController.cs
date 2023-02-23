using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //Атрибут, который дает доступ по указанному пути.
    [Route("/api/[controller]")]
    public class ProductController : Controller
    {
        private static List<Product> products = new List<Product>(new[]
        {
            new Product() {Id = 1, Name = "Notebook", Price = 10000},
            new Product() {Id = 2, Name = "Car", Price = 2000000},
            new Product() {Id = 3, Name = "Appke", Price = 30},
        });

        //При обращении к этому контроллеру по этому get без параметров, будет вызван метод get.
        [HttpGet]
        //Метод для запроса Get
        public IEnumerable<Product> Get()
        {
            return products;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //Метод линкью. Пробегается по товару и ищет соответствующий id у товара.
            var product = products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            products.Remove(products.SingleOrDefault(p => p.Id == id));
            return Ok(new { Message = "Deleted successfully" });
        }

        //Уникальный идентификатор к добавленному товару
        private int NextProductId => products.Count() == 0 ? 1 : products.Max(x => x.Id) + 1;
     

        [HttpGet("GetNextProductId")] //Проверка: /api/GetNextProductId/
        public int GetNextProductId()
        {
            return NextProductId;
        }

        [HttpPost]
        public IActionResult Post (Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            product.Id = NextProductId;
            products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);

        }

        [HttpPost("AddProduct")]
        public IActionResult PostBody([FromBody] Product product) => Post(product);

        //Изменение товара
        [HttpPut]
        public IActionResult Put(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var storedProduct = products.SingleOrDefault(p => p.Id == product.Id);
            if (storedProduct == null) return NotFound();
            storedProduct.Name = product.Name;
            storedProduct.Price = product.Price;
            return Ok(storedProduct);
        }
        [HttpPut("UpdateProduct")]
        public IActionResult PutBody([FromBody] Product product) => Put(product);


        //public String Index()
        //{
        //    return "sdfasdf";
        //}
    }
}
