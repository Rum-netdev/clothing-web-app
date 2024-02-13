using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Mappers
{
    public class ProductMapper : BaseEntityMapperProfile
    {
        public override void ConfigureMapping()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
