using Microsoft.Extensions.Options;
using SignalRAPI.DTOs.Config;
using SignalRAPI.Hubs;
using SignalRAPI.Models;
using TableDependency.SqlClient;

namespace SignalRAPI.SubscribeTableDependencies
{
    public interface ISubscribeTableDependency
    {
        void SubscribeTableDependency();
    }
    public class SubscribeEmployeeTableDependency : ISubscribeTableDependency
    {
        private readonly EmployeeHub _chatHub;
        private readonly IOptionsMonitor<ConnectionStrings> _connectionString;
        private readonly ILogger<SubscribeEmployeeTableDependency> _logger;

        public SubscribeEmployeeTableDependency(EmployeeHub chatHub, IOptionsMonitor<ConnectionStrings> connectionString, ILogger<SubscribeEmployeeTableDependency> logger)
        {
            _chatHub = chatHub;
            _connectionString = connectionString;
            _logger = logger;
        }

        public void SubscribeTableDependency()
        {
            var tableDependency = new SqlTableDependency<TblClaysysEmployee>(_connectionString.CurrentValue.DbConnection, tableName: "tblClaysysEmployees", executeUserPermissionCheck: false);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<TblClaysysEmployee> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await _chatHub.UpdateEmployeeCountsToUser();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            _logger.LogError($"{nameof(TblClaysysEmployee)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
