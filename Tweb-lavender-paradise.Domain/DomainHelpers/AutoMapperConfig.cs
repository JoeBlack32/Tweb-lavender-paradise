using AutoMapper;
using Tweb_lavender_paradise.Domain.Enitities;
using Tweb_lavender_paradise.Domain.Models;
using Tweb_lavender_paradise.Domain.Entities;

namespace Tweb_lavender_paradise.Domain.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper MapperInstance { get; private set; }

        public static void RegisterMappings()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderHistory, OrderHistoryDBTable>();
                cfg.CreateMap<OrderHistoryDBTable, OrderHistory>();

                // Добавляй другие маппинги по мере необходимости
            });

            MapperInstance = config.CreateMapper();
        }
    }
}