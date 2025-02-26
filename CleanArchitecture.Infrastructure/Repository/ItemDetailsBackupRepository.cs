using CleanArchitecture.Application.Interface;
using CleanArchitecture.Infrastructure.Data;
using Dapper;
using Domain_Layer.Modal;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class ItemDetailsBackupRepository : IItemDetailsBackupRepository
    {
        private readonly DbContext _sqlServerDbContext;
        private readonly MySqlDbContext _dbContext;

        public ItemDetailsBackupRepository(MySqlDbContext dbContext, DbContext sqlServerDbContext)
        {
            _dbContext = dbContext;
            _sqlServerDbContext = sqlServerDbContext;
        }

        public async Task<int> BackupItemDetailAsync(int detailId)
        {

            using (var connection = _dbContext.CreateConnection())
            using (var sqlServerConnection = _sqlServerDbContext.CreateConnection())
            {
                using (var mysqlTransaction = connection.BeginTransaction())
                using (var sqlServerTransaction = sqlServerConnection.BeginTransaction())
                {
                    try
                    {
                        var getItemDetailQuery = "SELECT * FROM ItemDetails WHERE detail_id = @DetailId";
                        var itemDetail = await sqlServerConnection.QuerySingleOrDefaultAsync<ItemDetail>(getItemDetailQuery, new { DetailId = detailId }, sqlServerTransaction);

                        if (itemDetail == null)
                        {
                            throw new KeyNotFoundException("Item Details not found.");
                        }

                        var backupQuery = "INSERT INTO ItemDetailsBackup (item_id, detail_description) VALUES (@ItemId, @DetailDescription)";
                        await connection.ExecuteAsync(backupQuery, new { itemDetail.ItemId, itemDetail.DetailDescription }, mysqlTransaction);

                        var deleteQuery = "DELETE FROM ItemDetails WHERE detail_id = @DetailId";
                        var deleteResult = await sqlServerConnection.ExecuteAsync(deleteQuery, new { DetailId = detailId }, sqlServerTransaction);

                        if (deleteResult == 0)
                        {
                            throw new Exception("Failed to delete ItemDetail from SQL Server.");
                        }

                        mysqlTransaction.Commit();
                        sqlServerTransaction.Commit();

                        return deleteResult;
                    }
                    catch
                    {
                        mysqlTransaction.Rollback();
                        sqlServerTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<ItemDetail>> GetAllBackedUpDetailsAsync()
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = "SELECT * FROM ItemDetailsBackup";
                    return await connection.QueryAsync<ItemDetail>(query);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
