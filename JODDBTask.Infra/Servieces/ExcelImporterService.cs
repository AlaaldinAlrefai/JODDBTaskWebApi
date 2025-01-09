using JODDBTask.Core.Data;
using JODDBTask.Core.IReposetory;
using JODDBTask.Core.IServieces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace JODDBTask.Infra.Servieces
{
    public class ExcelImporterService : IExcelImporterService
    {
        private readonly IExcelDataRepository _excelDataRepository;

        public ExcelImporterService(IExcelDataRepository excelDataRepository)
        {
            _excelDataRepository = excelDataRepository;
        }
        public async Task ImportExcelAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                var bulkData = new List<ExcelDataModel>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var data = new ExcelDataModel
                    {
                        NameId = worksheet.Cells[row, 1].GetValue<int>(),
                        Name = worksheet.Cells[row, 2].Text,
                        Email = worksheet.Cells[row, 3].Text,
                        MobileNo = worksheet.Cells[row, 4].Text
                    };

                    bulkData.Add(data);

                    if (bulkData.Count >= 1000)
                    {
                        await _excelDataRepository.InsertDataAsync(bulkData);
                        bulkData.Clear();
                    }
                }

                if (bulkData.Count > 0)
                {
                    await _excelDataRepository.InsertDataAsync(bulkData);
                }
            }
        }


    }
}
