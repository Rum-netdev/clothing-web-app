using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Services
{
    public interface IDapperService
    {
        T ExecuteQuery<T>(string query) where T : class;
        void ExecuteQuery(string query);
    }

    public class DapperService : IDapperService
    {
        private readonly SqlConnection _sqlConnection;

        public DapperService(IConfiguration configuration)
        {
            string connectString = configuration.GetConnectionString("Default");
            _sqlConnection = new SqlConnection(connectString);
        }

        public T ExecuteQuery<T>(string query) where T : class
        {
            throw new NotImplementedException();
        }

        public void ExecuteQuery(string query)
        {
            throw new NotImplementedException();
        }
    }
}
