using AutoMapper;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
        //public override void ConfigureMapping()
        //{
        //    CreateMap<Product, ProductDto>();
        //    CreateMap<ProductDto, Product>();
        //}
    }
}
