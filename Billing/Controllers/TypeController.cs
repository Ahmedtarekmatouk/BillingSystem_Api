using Billing.DTO.TypeDTO;
using Billing.Models;
using Billing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Billing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        
        [HttpGet]
        public IActionResult GetAll()
        {
            var types = _typeService.GetAll();
            if (types == null)
            {
                return NotFound();
            }
            return Ok(types);
        }

        
        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var type = _typeService.GetById(id);

            if (type == null)
            {
                return NotFound($"Type with ID {id} not found.");
            }

            return Ok(type);
        }

        [HttpPost]
        public IActionResult Create(TypeDTO type)
        {
            if (type == null) 
            {
                return BadRequest("some data is missing");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdType = _typeService.Create(type);
            if (createdType == null)
            {
                return BadRequest("Failed to create the Type. Type with the same name might already exist.");
            }

            return Ok(createdType);
        }


        [HttpPut("{id}")]
        public ActionResult<Billing.Models.Type> Update(int id, TypeDTO updatedType)
        {
            if (updatedType==null)
            {
                return BadRequest("some Data is missing");
            }

            var existingType = _typeService.GetById(id);
            if (existingType == null)
            {
                return NotFound($"Type with ID {id} not found.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _typeService.Update(updatedType);

            return Ok(updatedType);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var type = _typeService.GetById(id);

            if (type == null)
            {
                return NotFound($"Type with ID {id} not found.");
            }

            _typeService.Delete(id);
            return NoContent();
        }

    }
}
