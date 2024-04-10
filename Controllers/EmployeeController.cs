using Microsoft.AspNetCore.Mvc;
using SignalRAPI.DTOs.Request;
using SignalRAPI.Repositories;

namespace SignalRAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("GetEmployeeCount")]
        public async Task<IActionResult> GetEmployeeCount()
        {
            var response = await _employeeRepository.GetEmployeesCount();
            return Ok(response);
        }

        [HttpPost]
        [Route("InsertEmployee")]
        public async Task<IActionResult> InsertEmployee(AddEmployeeRequest request)
        {
            var response = await _employeeRepository.InsertEmployees(request);
            return Ok(response);
        }
    }
}
