using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SignalRAPI.DbContexts;
using SignalRAPI.DTOs.Config;
using SignalRAPI.DTOs.Request;
using SignalRAPI.DTOs.Response;
using SignalRAPI.Models;

namespace SignalRAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<int> GetEmployeesCount();
        Task<AddEmployeeResponse> InsertEmployees(AddEmployeeRequest request);
        Task<bool> IsTableExists();
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptionsMonitor<ConnectionStrings> _connectionString;

        public EmployeeRepository(ApplicationDbContext context, IOptionsMonitor<ConnectionStrings> connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<int> GetEmployeesCount()
        {
            try
            {
                var count = await _context.TblClaysysEmployees.CountAsync();

                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AddEmployeeResponse> InsertEmployees(AddEmployeeRequest request)
        {
            try
            {
                var tblClaysysEmployeeRequest = new TblClaysysEmployee
                {
                    Name = request.Name,
                    Department = request.Department
                };

                _context.TblClaysysEmployees.Add(tblClaysysEmployeeRequest);

                await _context.SaveChangesAsync();

                return new() { IsSuccess = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsTableExists()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString.CurrentValue.DbConnection))
                {
                    await connection.OpenAsync();

                    var tableName = nameof(_context.TblClaysysEmployees);
                    var schemaQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";

                    using (var command = new SqlCommand(schemaQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", tableName);

                        var result = await command.ExecuteScalarAsync();
                        var count = result is not null ? (int)result : 0;

                        return count > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
