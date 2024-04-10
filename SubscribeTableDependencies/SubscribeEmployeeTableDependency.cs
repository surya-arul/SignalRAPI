using SignalRAPI.Hubs;
using SignalRAPI.Models;
using TableDependency.SqlClient;

namespace SignalRAPI.SubscribeTableDependencies
{
    public interface ISubscribeTableDependency
    {
        void SubscribeTableDependency(string connectionString);
    }
    public class SubscribeEmployeeTableDependency : ISubscribeTableDependency
    {
        private readonly EmployeeHub _chatHub;
        SqlTableDependency<TblClaysysEmployee> _tableDependency = null!;

        private readonly ILogger<SubscribeEmployeeTableDependency> _logger;

        public SubscribeEmployeeTableDependency(EmployeeHub chatHub, ILogger<SubscribeEmployeeTableDependency> logger)
        {
            _chatHub = chatHub;
            _logger = logger;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            _tableDependency = new SqlTableDependency<TblClaysysEmployee>(connectionString,tableName:"tblClaysysEmployees", executeUserPermissionCheck: false);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
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
