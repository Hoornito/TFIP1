using Shared.AppSettings;
using Shared.Models;

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ConnectionStrings _connectionStringsConfig;

        public DocumentRepository(ConnectionStrings connectionStringsConfig)
        {
            _connectionStringsConfig = connectionStringsConfig;
        }

        public async Task<int> Insert(DocumentInfo document)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionStringsConfig.SqlConnection))
                {
                    string insertQuery = @"INSERT INTO DocumentTable (Name, InsertDate, PrintDate, Status) VALUES (@Name, @InsertDate, @PrintDate, @Status)";

                    var result = await db.ExecuteAsync(insertQuery, document, commandType: CommandType.Text);

                    return result;
                    //db.Query("INSERT INTO DocumentTable (Name, InsertDate, PrintDate, Status) VALUES (@Name, @InsertDate, @PrintDate, @Status)");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<DocumentInfo> GetDocument(string name)
        {
            using (IDbConnection db = new SqlConnection(_connectionStringsConfig.SqlConnection))
            {
                string selectQuery = @"SELECT * FROM DocumentTable WHERE Name = @Name";

                var result = await db.QueryAsync<DocumentInfo>(selectQuery, new { Name = name });

                return result.First();
            }
        }   
    }

    public interface IDocumentRepository
    {
        public Task<int> Insert(DocumentInfo documentInfo);
        public Task<DocumentInfo> GetDocument(string name);
    }
}
