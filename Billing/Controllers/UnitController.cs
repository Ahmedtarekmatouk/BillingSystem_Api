using Billing.DTO.UnitsDTOS;
using Billing.Models;
using Billing.Services.Units;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Billing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class UnitController : ControllerBase
    {
        IUnitService unitService;
        public UnitController(IUnitService unitService)
        {
            this.unitService = unitService;
        }
        [HttpGet]
        public IActionResult DisplayAll(string search = "", string sortField = "Name", string sortOrder = "asc", int page = 1, int pageSize = 10)
        {

            var allUnits = unitService.GetAll();

            if (allUnits == null || !allUnits.Any())
            {
                return NotFound("No units found.");
            }


            if (!string.IsNullOrEmpty(search))
            {
                allUnits = allUnits
                    .Where(u => u.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                u.Notes.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }


            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToLower())
                {
                    case "name":
                        allUnits = sortOrder.ToLower() == "asc"
                            ? allUnits.OrderBy(u => u.Name).ToList()
                            : allUnits.OrderByDescending(u => u.Name).ToList();
                        break;
                    case "notes":
                        allUnits = sortOrder.ToLower() == "asc"
                            ? allUnits.OrderBy(u => u.Notes).ToList()
                            : allUnits.OrderByDescending(u => u.Notes).ToList();
                        break;
                    default:
                        return BadRequest("Invalid sort field.");
                }
            }


            var pagedUnits = allUnits.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var unitDTOs = pagedUnits.Select(u => new UnitDTO
            {
                Id = u.Id,
                Name = u.Name,
                Notes = u.Notes
            }).ToList();

            return Ok(unitDTOs);
        }

        [HttpGet("{id:int}")]
        public ActionResult DisplayById(int id)
        {
            var unit = unitService.GetById(id);
            var dto = new UnitDTO();
            if (unit != null)
            {

                dto.Name = unit.Name;
                dto.Id = unit.Id;
                dto.Notes = unit.Notes;
                return Ok(dto);
            }
            return NotFound("this unit is not found");


        }
        [HttpPost]
        public ActionResult Create([FromBody] UnitDTO dto)
        {
            // Check if the dto is null
            if (dto == null)
            {
                return BadRequest("Some data is missing.");
            }

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                var unit = new Unit
                {
                    Name = dto.Name,
                    Notes = dto.Notes
                };

                
                var createdUnit = unitService.Create(unit);

                
                var createdDto = new UnitDTO
                {
                    Id = createdUnit.Id,
                    Name = createdUnit.Name,
                    Notes = createdUnit.Notes
                };

               
                return CreatedAtAction(nameof(DisplayById), new { id = createdUnit.Id }, createdDto);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while creating the unit. " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Edit(int id, [FromBody] UnitDTO dto)
        {
           
            if (dto == null)
            {
                return BadRequest("Some data is missing.");
            }

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                var unit = unitService.GetById(id);
                if (unit == null)
                {
                    return NotFound("The unit was not found.");
                }

                
                unit.Name = dto.Name;
                unit.Notes = dto.Notes;

              
                var updatedUnit = unitService.Update(unit);

              
                var updatedDto = new UnitDTO
                {
                    Id = updatedUnit.Id,
                    Name = updatedUnit.Name,
                    Notes = updatedUnit.Notes
                };

                
                return Ok(updatedDto);
            }
            catch (Exception ex)
            {
                 
                return StatusCode(500, "An error occurred while updating the unit. " + ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var unit = unitService.GetById(id);
            if (unit == null)
            {
                return NotFound("this unit is not found");

            }
            unitService.Delete(id);
            return NoContent();

        }
    }
}
