using Microsoft.AspNetCore.SignalR;
using SignalRAPI.Repositories;

namespace SignalRAPI.Hubs
{
    public class EmployeeHub : Hub
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeHub(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Hello, Hub is connected");
        }

        public async Task UpdateEmployeeCountsToUser()
        {
            var response = await _employeeRepository.GetEmployeesCount();

            await Clients.All.SendAsync($"Current count of employees: {response}", response);
        }
    }
}
