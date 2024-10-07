
using Billing.DTO;
using Billing.Models;
using Billing.Services.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Billing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService service;
        public InvoiceController(IInvoiceService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] InvoiceDTO invoiceDTO)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            Invoice invoice = service.Create(invoiceDTO);
            if (invoice == null)
                return StatusCode(500);

           
            InvoiceDTO response = service.GetByIdAsDTO(invoice.Id);
            return CreatedAtAction("GetById", new { id = invoice.Id },response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            InvoiceDTO invoice = service.GetByIdAsDTO(id);
            if (invoice != null)
            {
                return Ok(invoice);
            }
            return NotFound();
        }

        [HttpPut]
        public IActionResult Update([FromBody] InvoiceDTO invoiceFromReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool invoice = service.Update(invoiceFromReq);
            if (!invoice)
                return NotFound();
            else
                return Ok();
        }

        [HttpGet]
        public IActionResult GetAll(string search = null, string sortBy = null, bool sortDesc = false, int page = 1, int pageSize = 10)
        {  
            List<InvoiceDTO> invoices = service.GetAll(search,sortBy,sortDesc,page,pageSize);
            if (invoices == null || invoices.Count == 0)
                return NoContent(); 

            return Ok(invoices);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (service.GetById(id) == null)
                return NotFound();

            service.Delete(id);   
                return Ok();
           
        }


    }
}
