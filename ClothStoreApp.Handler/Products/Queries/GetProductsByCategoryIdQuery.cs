using AutoMapper;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Products.Dtos;
using ClothStoreApp.Handler.Responses;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ClothStoreApp.Handler.Products.Queries
{
    public class GetProductsByCategoryIdQuery : IQuery<GetProductsByCategoryIdQueryResult>
    {
        public int CategoryId { get; set; }
    }

    public class GetProductsByCategoryIdQueryHandler : IQueryHandler<GetProductsByCategoryIdQuery, GetProductsByCategoryIdQueryResult>
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private SqlConnection _sqlConnection;

        public GetProductsByCategoryIdQueryHandler(IConfiguration configuration,
            IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<GetProductsByCategoryIdQueryResult> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            GetProductsByCategoryIdQueryResult result = new();

            string requestQuery = $"select";

            PropertyInfo[] dtoProps = typeof(ProductDto).GetProperties();
            foreach(PropertyInfo prop in dtoProps)
            {
                requestQuery += $" {prop.Name},";
            }
            requestQuery = requestQuery.TrimEnd(',');
            requestQuery += $" from {nameof(Product)}s";
            requestQuery += $" where {nameof(ProductDto.CategoryId)} = {request.CategoryId}";

            using (_sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var dtos = (await _sqlConnection.QueryAsync<ProductDto>(requestQuery)).ToList();
                result.Data = dtos;
                result.TotalRecords = dtos.Count;
                result.Message = $"Retrive {dtos.Count} entities from Products successfully";
                result.IsSuccess = true;
            }

            return result;
        }
    }

    public class GetProductsByCategoryIdQueryResult : BaseResult<ICollection<ProductDto>>
    {
        public int TotalRecords { get; set; }
    }
}
