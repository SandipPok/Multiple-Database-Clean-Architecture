using CleanArchitecture.Application.Interface;
using CleanArchitecture.Infrastructure.Data;
using Dapper;
using Domain_Layer.Modal;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class ItemDetailsRepository : IItemDetailsRepository
    {
        private readonly DbContext _dbContext;

        public ItemDetailsRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateItemDetailAsync(ItemDetail itemDetail)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = @"INSERT INTO ItemDetails (item_id, detail_description) 
                                VALUES (@ItemId, @DetailDescription)";
                    var parameters = new { ItemId = itemDetail.ItemId, DetailDescription = itemDetail.DetailDescription };
                    var result = await connection.ExecuteAsync(query, parameters);
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        //public async Task<int> DeleteItemDetailAsync(int detailId)
        //{
        //    try
        //    {
        //        using (var connection = _dbContext.CreateConnection())
        //        {
        //            var query = @"DELETE FROM ItemDetails 
        //                            WHERE detail_id = @DetailId";
        //            var parameters = new { DetailId = detailId };
        //            var result = await connection.ExecuteAsync(query, parameters);
        //            return result;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<ItemDetail>> GetDetailsByItemIdAsync(int itemId)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var query = @"SELECT * FROM ItemDetails 
                                    WHERE item_id = @ItemId";
                    var parameters = new { ItemId = itemId };
                    var result = await connection.QueryAsync<ItemDetail>(query, parameters);
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
