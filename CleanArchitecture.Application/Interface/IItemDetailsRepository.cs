using Domain_Layer.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interface
{
    public interface IItemDetailsRepository
    {
        Task<IEnumerable<ItemDetail>> GetDetailsByItemIdAsync(int itemId);
        Task<int> CreateItemDetailAsync(ItemDetail itemDetail);
        //Task<int> DeleteItemDetailAsync(int detailId);
    }
}
