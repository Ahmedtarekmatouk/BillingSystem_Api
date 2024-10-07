using Billing.DTO.CompanyDTO;
using Billing.Models;
using Billing.Services.Compaines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Billing.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        ICompanyService companyService;
        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;

        }
        [HttpGet]
        public ActionResult DisplayAll(string search = "", string sortField = "CompanyName", string sortOrder = "asc", int page = 1, int pageSize = 10)
        {

            var allCompanies = companyService.GetAll();

            if (allCompanies == null)
            {
                return NotFound();
            }


            if (!string.IsNullOrEmpty(search))
            {
                allCompanies = allCompanies.Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                       c.Notes.Contains(search, StringComparison.OrdinalIgnoreCase))
                                           .ToList();
            }


            switch (sortField.ToLower())
            {
                case "companyname":
                    allCompanies = sortOrder.ToLower() == "asc"
                        ? allCompanies.OrderBy(c => c.Name).ToList()
                        : allCompanies.OrderByDescending(c => c.Name).ToList();
                    break;
                case "notes":
                    allCompanies = sortOrder.ToLower() == "asc"
                        ? allCompanies.OrderBy(c => c.Notes).ToList()
                        : allCompanies.OrderByDescending(c => c.Notes).ToList();
                    break;
                default:
                    allCompanies = allCompanies.OrderBy(c => c.Name).ToList();
                    break;
            }


            var pagedCompanies = allCompanies.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            List<CompanyDTO> dtos = pagedCompanies.Select(company => new CompanyDTO
            {
                Id = company.Id,
                CompanyName = company.Name,
                Notes = company.Notes,
            }).ToList();


            return Ok(dtos);

        }
        [HttpGet("{id:int}")]
        public ActionResult DisplayById(int id)
        {
            var Comp = companyService.GetById(id);
            CompanyDTO Dto = new CompanyDTO();
            if (Comp != null)
            {
                Dto.Id = Comp.Id;
                Dto.CompanyName = Comp.Name;
                Dto.Notes = Comp.Notes;

                return Ok(Dto);
            }
            return NotFound();

        }
        [HttpPost]
        public ActionResult Create([FromBody] CompanyDTO companyDTO)
        {
            
            if (companyDTO == null)
            {
                return BadRequest("Company data is missing.");
            }

           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            try
            {
                
                var company = new Company
                {
                    Name = companyDTO.CompanyName,
                    Notes = companyDTO.Notes
                };

                
                var createdCompany = companyService.Create(company);

                
                var createdCompanyDTO = new CompanyDTO
                {
                    Id = createdCompany.Id,
                    CompanyName = createdCompany.Name,
                    Notes = createdCompany.Notes
                };

                
                return CreatedAtAction(nameof(DisplayById), new { id = createdCompanyDTO.Id }, createdCompanyDTO);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while creating the company. " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] CompanyDTO companyDTO)
        {
            
            if (companyDTO == null)
            {
                return BadRequest("Company data is null.");
            }

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                var existingCompany = companyService.GetById(id);
                if (existingCompany == null)
                {
                    return NotFound("Company not found.");
                }

                
                existingCompany.Name = companyDTO.CompanyName;
                existingCompany.Notes = companyDTO.Notes;

                
                var updatedCompany = companyService.Update(existingCompany);

                
                var updatedCompanyDTO = new CompanyDTO
                {
                    Id = updatedCompany.Id,
                    CompanyName = updatedCompany.Name,
                    Notes = updatedCompany.Notes
                };

                
                return Ok(updatedCompanyDTO);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while updating the company. " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var company = companyService.GetById(id);
            if (company == null)
            {
                return NotFound("Company not found.");
            }

            companyService.Delete(id);

            return NoContent();
        }


    }
}
