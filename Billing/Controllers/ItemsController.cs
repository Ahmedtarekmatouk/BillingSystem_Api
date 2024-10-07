using Billing.DTO;
using Billing.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Billing.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        IItemsService itemsService;
        public ItemsController(IItemsService itemsService)
        {
            this.itemsService = itemsService;
        }
        [HttpGet]
        public IActionResult GetAll(string search = null, string sortBy = null, bool sortDesc = false, int page = 1, int pageSize = 10) 
        {
             if (itemsService.GetAll(search,sortBy,sortDesc,page,pageSize)==null)
                return NotFound();
             return Ok(itemsService.GetAll(search, sortBy, sortDesc, page, pageSize));
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id) 
        {
            if (itemsService.GetById(id)==null)
                return NotFound(); 
            return Ok(itemsService.GetById(id));
        }
        [HttpGet("GetItemsByTypeID/{id:int}")]
        public IActionResult GetItemsByTypeID(int id)
        {
            if (itemsService.GetItemsByTypeID(id)==null)
                return NotFound();
            return Ok(itemsService.GetItemsByTypeID(id));
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteById(int id) {
            itemsService.Delete(id);
            itemsService.save();
            return Ok();
        }

        [HttpPost]
        public IActionResult Create([FromBody] ItemsDTO itemsDTO)
        {
           
            if (itemsDTO == null)
            {
                return BadRequest("Item data is missing.");
            }

           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                ItemsDTO createdItemDTO = itemsService.Create(itemsDTO);

                
                if (createdItemDTO == null)
                {
                    return BadRequest("Failed to create the item. Item with the same name might already exist.");
                }

                
                return CreatedAtAction(nameof(GetById), new { id = createdItemDTO.Id }, createdItemDTO);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"An error occurred while creating the item. {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(ItemsDTO itemsDTO) 
        {
            if (itemsDTO == null)
            {
                return NotFound("the item is mssing");
            }
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }
            ItemsDTO itemDTO= itemsService.Update(itemsDTO);
            if (itemDTO == null)
                return NotFound();
           return Ok(itemDTO);
        }
    }
}
