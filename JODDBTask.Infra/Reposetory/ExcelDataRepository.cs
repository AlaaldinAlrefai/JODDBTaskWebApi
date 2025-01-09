using JODDBTask.Core.Data;
using JODDBTask.Core.IReposetory;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JODDBTask.Infra.Reposetory
{
    public class ExcelDataRepository : IExcelDataRepository
    {
        private readonly string _connectionString;

        public ExcelDataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task InsertDataAsync(List<ExcelDataModel> dataBatch)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Users";

                    bulkCopy.ColumnMappings.Add("NameId", "Id");
                    bulkCopy.ColumnMappings.Add("Name", "Name");
                    bulkCopy.ColumnMappings.Add("Email", "Email");
                    bulkCopy.ColumnMappings.Add("MobileNo", "MobileNumber");
                    bulkCopy.ColumnMappings.Add("Password", "Password");

                    var dataTable = new System.Data.DataTable();
                    dataTable.Columns.Add("NameId");
                    dataTable.Columns.Add("Name");
                    dataTable.Columns.Add("Email");
                    dataTable.Columns.Add("MobileNo");
                    dataTable.Columns.Add("Password");


                    foreach (var item in dataBatch)
                    {
                        string password = GenerateRandomPassword(12);
                        dataTable.Rows.Add(item.NameId, item.Name, item.Email,item.MobileNo, password);
                    }

                    await bulkCopy.WriteToServerAsync(dataTable);
                }
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";
            StringBuilder password = new StringBuilder();
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[length];
                rng.GetBytes(buffer);

                for (int i = 0; i < length; i++)
                {
                    password.Append(validChars[buffer[i] % validChars.Length]);
                }
            }
            return password.ToString();
        }
    }
}
