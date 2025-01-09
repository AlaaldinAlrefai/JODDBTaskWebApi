using JODDBTask.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JODDBTask.Core.IReposetory
{
    public interface IExcelDataRepository
    {
        Task InsertDataAsync(List<ExcelDataModel> dataBatch);
    }
}
