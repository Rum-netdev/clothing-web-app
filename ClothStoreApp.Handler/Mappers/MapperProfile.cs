using AutoMapper;

namespace ClothStoreApp.Handler.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //ApplyConfigurationFromAssembly();
        }

        //private void ApplyConfigurationFromAssembly()
        //{
        //    Type type = typeof(BaseEntityMapperProfile);
        //    var implements = AppDomain.CurrentDomain.GetAssemblies()
        //        .Select(s => s.GetType())
        //        .Where(p => type.IsAssignableFrom(p));

        //    foreach(var implement in implements)
        //    {
        //        implement.GetMethod("ConfigureMapping").Invoke(null, null);
        //    }
        //}
    }
}
