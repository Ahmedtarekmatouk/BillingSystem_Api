using Billing.DTO.EmployeeDTO;
using Billing.Models;
using Billing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Billing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Tokens _tokenService;

        public EmployeeController(UserManager<ApplicationUser> userManager, Tokens tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewEmployee(NewEmployeeDTO newEmployee)
        {
            if (ModelState.IsValid)
            {
                var normalizedUserName = newEmployee.EmployeeName.ToUpper();
                var existingEmployee =  _tokenService.CheckUserNameExists(normalizedUserName);

                if (existingEmployee)
                {
                    return BadRequest("Username already exists.");
                }

                var employee = new ApplicationUser
                {
                    UserName = newEmployee.EmployeeName,
                    Id = Guid.NewGuid().ToString(),
                    PhoneNumber = newEmployee.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(employee, newEmployee.Password);

                if (result.Succeeded)
                {
                    return Ok("The new employee has been added successfully.");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.EmployeeName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await _tokenService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password");
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<EmployeeDTO>> GetAllEmployees()
        {
            var employees = _tokenService.GetAllUsers()
                .Select(user => new EmployeeDTO
                {
                    Id = user.Id,
                    Name = user.UserName,
                    PhoneNumbe = user.PhoneNumber
                }).ToList();

            return Ok(employees);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteEmployee(string id)
        {
            var result = _tokenService.DeleteUser(id);

            if (result)
            {
                return Ok("Employee deleted successfully.");
            }

            return BadRequest("Failed to delete employee.");
        }
    }
}
