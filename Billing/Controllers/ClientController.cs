using Billing.DTO.ClientsDTOS;
using Billing.Models;
using Billing.Services.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Billing.Controllers
{
<<<<<<< HEAD
    //[Authorize]
=======
    
>>>>>>> dafac5c78cb193e10625f4ddc5e4a7230d0f10d4
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        IClientService clientService;
        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }
        [HttpGet]
        public IActionResult DisplayAll(string search = "", string sortField = "Name", string sortOrder = "asc", int page = 1, int pageSize = 10)
        {
            
            var allClients = clientService.GetAll();

            if (allClients == null || !allClients.Any())
            {
                return NotFound("No clients found.");
            }

           
            if (!string.IsNullOrEmpty(search))
            {
                allClients = allClients
                    .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                c.Address.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                c.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) 
                                )
                    .ToList();
            }

            
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToLower())
                {
                    case "name":
                        allClients = sortOrder.ToLower() == "asc"
                            ? allClients.OrderBy(c => c.Name).ToList()
                            : allClients.OrderByDescending(c => c.Name).ToList();
                        break;
                    case "address":
                        allClients = sortOrder.ToLower() == "asc"
                            ? allClients.OrderBy(c => c.Address).ToList()
                            : allClients.OrderByDescending(c => c.Address).ToList();
                        break;
                    case "phone":
                        allClients = sortOrder.ToLower() == "asc"
                            ? allClients.OrderBy(c => c.Phone).ToList()
                            : allClients.OrderByDescending(c => c.Phone).ToList();
                        break;
                    case "number":
                        allClients = sortOrder.ToLower() == "asc"
                            ? allClients.OrderBy(c => c.Number).ToList()
                            : allClients.OrderByDescending(c => c.Number).ToList();
                        break;
                    default:
                        return BadRequest("Invalid sort field.");
                }
            }

          
            var pagedClients = allClients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            
            var clientDTOs = pagedClients.Select(client => new ClientDTO
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Phone = client.Phone,
                Number = client.Number
            }).ToList();

            return Ok(clientDTOs);
        }
        [HttpGet("{id:int}")]
        public IActionResult DisplayById(int id)
        {
            var client = clientService.GetById(id);

            if (client == null)
            {
                return NotFound($"No client found with ID: {id}");
            }

            var clientDTO = new ClientDTO
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Phone = client.Phone,
                Number = client.Number
            };

            return Ok(clientDTO);
        }


        [HttpPost]
        public ActionResult Create([FromBody] ClientDTO clientDTO)
        {
            
            if (clientDTO == null)
            {
                return BadRequest("Client data is missing.");
            }

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            
            var client = new Client
            {
                Name = clientDTO.Name,
                Address = clientDTO.Address,
                Phone = clientDTO.Phone
            };

            try
            {
                
                var createdClient = clientService.Create(client);

                
                var dto = new ClientDTO
                {
                    Id = createdClient.Id,
                    Name = createdClient.Name,
                    Address = createdClient.Address,
                    Phone = createdClient.Phone,
                    Number = createdClient.Number
                };

                
                return CreatedAtAction(nameof(DisplayById), new { id = createdClient.Id }, dto);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while creating the client. " + ex.Message);
            }
        

    }
        [HttpPut("{id}")]
        public ActionResult Edit(int id, [FromBody] ClientDTO clientDTO)
        {
            
            if (clientDTO == null)
            {
                return BadRequest("Client data is missing.");
            }

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

         
            var existingClient = clientService.GetById(id);
            if (existingClient == null)
            {
                return NotFound("The client was not found.");
            }

            
            existingClient.Name = clientDTO.Name;
            existingClient.Address = clientDTO.Address;
            existingClient.Phone = clientDTO.Phone;
            existingClient.Number = clientDTO.Number;

            try
            {
                
                var updatedClient = clientService.Update(existingClient);

                
                var updatedClientDTO = new ClientDTO
                {
                    Id = updatedClient.Id,
                    Name = updatedClient.Name,
                    Address = updatedClient.Address,
                    Phone = updatedClient.Phone,
                    Number = updatedClient.Number
                };

               
                return Ok(updatedClientDTO);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while updating the client. " + ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var client = clientService.GetById(id);
            if (client == null)
            {
                return NotFound("Client not found.");
            }
            clientService.Delete(id);

            return NoContent();
        }
        
    }
}

