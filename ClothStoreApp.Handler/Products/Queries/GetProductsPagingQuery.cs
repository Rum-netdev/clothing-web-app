using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ClothStoreApp.Handler.Products.Queries
{
    public class GetProductsPagingQuery : IQuery<GetProductsPagingQueryResult>
    {
        public string Keyword { get; set; } = "";
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }

    public class GetProductsPagingQueryHandler : IQueryHandler<GetProductsPagingQuery, GetProductsPagingQueryResult>
    {
        private SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public GetProductsPagingQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Use Dapper instead EF Core
        public async Task<GetProductsPagingQueryResult> Handle(GetProductsPagingQuery request, CancellationToken cancellationToken)
        {
            GetProductsPagingQueryResult result = new GetProductsPagingQueryResult(null, request.PageNumber, request.PageSize);
            // Validate input data

            // 2 options: direct query or calling Stored Procedures
            // Option 1:
            string query = $"select * from {nameof(Product)}s";

            var properties = typeof(Product).GetProperties();

            if(!string.IsNullOrEmpty(request.Keyword))
            {
                request.Keyword = request.Keyword.Replace("'", "''");
                query += $" where";
                foreach(PropertyInfo property in properties)
                {
                    if(property.PropertyType == typeof(string))
                    {
                        query += $" {property.Name}={request.Keyword}";
                    }
                }
            }

            query += $" order by {nameof(Product)}s.Id";

            if (request.PageSize < 10) request.PageSize = 10;
            if (request.PageNumber <= 0) request.PageNumber = 1;
            result.PageNumber = request.PageNumber;
            result.PageSize = request.PageSize;

            query += $" offset {(request.PageNumber - 1) * request.PageSize} rows fetch next {request.PageSize} rows only";

            using (_sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var products = (await _sqlConnection.QueryAsync<Product>(query)).ToList();
                result.Data = products;
                result.TotalPages = Convert.ToInt32(products.Count / request.PageSize);
                result.TotalRecords = products.Count;
            }

            return result;
        }
    }

    public class GetProductsPagingQueryResult : PaginationResult<ICollection<Product>>
    {
        public GetProductsPagingQueryResult(ICollection<Product> data, int pageNumber, int pageSize)
            :base(data, pageNumber, pageSize)
        {
        }
    }
}
