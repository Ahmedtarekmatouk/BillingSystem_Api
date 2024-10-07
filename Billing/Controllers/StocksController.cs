using Billing.DTO.StocksDTO;
using Billing.Models;
using Billing.Services.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class StocksController : ControllerBase
    {
        IStockService stockService;
        public StocksController(IStockService stockService)
        {
            this.stockService = stockService;
        }
        [HttpGet]
        public IActionResult DisplayAll(string search = "", string sortField = null, string sortOrder = "asc", int page = 1, int pageSize = 10)
        {
            
            var allStocks = stockService.GetAll();

            if (allStocks == null || !allStocks.Any())
            {
                return NotFound("There are no items in your stock.");
            }

           
            if (!string.IsNullOrEmpty(search))
            {
                allStocks = allStocks
                    .Where(s => s.Items != null && s.Items.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToLower())
                {
                    case "itemname":
                        allStocks = sortOrder.ToLower() == "asc"
                            ? allStocks.OrderBy(s => s.Items.Name).ToList()
                            : allStocks.OrderByDescending(s => s.Items.Name).ToList();
                        break;
                    case "quantity":
                        allStocks = sortOrder.ToLower() == "asc"
                            ? allStocks.OrderBy(s => s.Quntity).ToList()
                            : allStocks.OrderByDescending(s => s.Quntity).ToList();
                        break;
                    default:
                        return BadRequest("Invalid sort field.");
                }
            }

            
            var pagedStocks = allStocks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            
            var stocksDTOs = pagedStocks.Select(stock => new StocksDTO
            {
                Id = stock.Id,
                ItemName = stock.Items?.Name,
                Quntity = stock.Quntity
            }).ToList();

            return Ok(stocksDTOs);
        }

        [HttpGet("{id:int}")]
        public IActionResult DisplayById(int id)
        {
            var Stock = stockService.GetById(id);
            if (Stock != null)
            {
                StocksDTO stdto = new StocksDTO
                {
                    Id = Stock.Id,
                    ItemName = Stock.Items.Name,
                    Quntity = Stock.Quntity,

                };
                return Ok(stdto);
            }
            return NotFound("this item have no stock");
        }
        [HttpGet("Stock/{id:int}")]
        public IActionResult DisplayByItemId(int id)
        {
            var Stock = stockService.GetAll().FirstOrDefault(s=>s.ItemsId==id);
            if (Stock != null)
            {
                StocksDTO stdto = new StocksDTO
                {
                    Id = Stock.ItemsId,
                    ItemName = Stock.Items.Name,
                    Quntity = Stock.Quntity,

                };
                return Ok(stdto);
            }
            return NotFound("this item have no stock");
        }

        [HttpPost]
        public ActionResult Create([FromBody] StocksDTO dTO)
        {
            if (dTO == null)
            {
                return BadRequest("Some data is missing.");
            }
            var stock = new Stocks
            {
                Id = dTO.Id,
                Items = new Item { Name = dTO.ItemName },
                Quntity = dTO.Quntity,
            };
            var createdStock = stockService.Create(stock);
            var DTO = new StocksDTO
            {
                Id = createdStock.Id,
                ItemName = createdStock.Items.Name,
                Quntity = createdStock.Quntity,
            };
            return CreatedAtAction(nameof(DisplayById), new { id = createdStock.Id }, DTO);

        }
        [HttpPut("{id:int}")]
        public ActionResult Edit(int id, [FromBody] StocksDTO dTO)
        {
            if (dTO == null)
            {
                return BadRequest("You can't edit this stock.");
            }

            var existingStock = stockService.GetById(id);
            if (existingStock == null)
            {
                return NotFound("The item was not found.");
            }


            if (existingStock.Items != null)
            {
                existingStock.Items.Name = dTO.ItemName;
            }

            existingStock.Quntity = dTO.Quntity;

            stockService.Update(existingStock);

            var updatedStockDTO = new StocksDTO
            {
                Id = existingStock.Id,
                ItemName = existingStock.Items?.Name,
                Quntity = existingStock.Quntity
            };

            return Ok(updatedStockDTO);
        }

       


    }
}
