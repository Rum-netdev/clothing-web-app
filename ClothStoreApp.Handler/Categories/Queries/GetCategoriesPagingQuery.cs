using ClothStoreApp.Handler.Categories.Dtos;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ClothStoreApp.Handler.Categories.Queries
{
    public class GetCategoriesPagingQuery : IQuery<GetCategoriesPagingQueryResult>
    {
        public string Keyword { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetCategoriesPagingQueryHandler : IQueryHandler<GetCategoriesPagingQuery, GetCategoriesPagingQueryResult>
    {
        private SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;

        public GetCategoriesPagingQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GetCategoriesPagingQueryResult> Handle(GetCategoriesPagingQuery request, CancellationToken cancellationToken)
        {
            GetCategoriesPagingQueryResult result = new(null, request.PageNumber, request.PageSize); ;
            PropertyInfo[] properties = typeof(CategoryDto).GetProperties();
            PropertyInfo[] textProps = properties.Where(p => p.GetType() == typeof(string)).ToArray();

            string executeQuery = "select";
            foreach(var prop in properties)
            {
                executeQuery += $" {prop.Name}";
            }
            executeQuery += $" from Categories";

            if(!string.IsNullOrEmpty(request.Keyword))
            {
                executeQuery += " where";
                foreach(var prop in textProps)
                {
                    executeQuery += $" {prop.Name} = '{request.Keyword.Replace("'", "''")}' ||";
                }
                executeQuery += executeQuery.Substring(0, executeQuery.Length - 2).Trim();
            }

            string countQuery = executeQuery;
            if (request.PageNumber <= 0) request.PageNumber = 1;
            if (request.PageSize <= 0) request.PageSize = 10;
            result.PageNumber = request.PageNumber;
            result.PageSize = request.PageSize;

            executeQuery += $"offset {(request.PageNumber - 1) * request.PageSize} rows " +
                $"fetch next {request.PageSize} rows only";

            using (_sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var categoryDtos = (await _sqlConnection.QueryAsync<CategoryDto>(executeQuery)).ToList();
                result.Data = categoryDtos;
                result.TotalRecords = await _sqlConnection.ExecuteAsync(countQuery);
                int totalPages = result.TotalRecords / request.PageSize;
                result.TotalPages = totalPages > 0 ? totalPages : 1;
            }

            result.IsSuccess = true;
            result.Message = $"Retrieve {request.PageSize > result.TotalRecords ? result.TotalRecords > result.PageSize} entities successfully";
            return result;
        }
    }

    public class GetCategoriesPagingQueryResult : PaginationResult<ICollection<CategoryDto>>
    {
        public GetCategoriesPagingQueryResult(ICollection<CategoryDto> data,
            int pageNumber,
            int pageSize)
            :base(data, pageNumber, pageSize)
        {
        }
    }
}
