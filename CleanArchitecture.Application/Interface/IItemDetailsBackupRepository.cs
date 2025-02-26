using Domain_Layer.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interface
{
    public interface IItemDetailsBackupRepository
    {
        Task<int> BackupItemDetailAsync(int detailId);
        Task<IEnumerable<ItemDetail>> GetAllBackedUpDetailsAsync();
    }
}
