using AutoMapper;

namespace ClothStoreApp.Handler.Mappers
{
    public abstract class BaseEntityMapperProfile : Profile
    {
        public abstract void ConfigureMapping();
    }
}
