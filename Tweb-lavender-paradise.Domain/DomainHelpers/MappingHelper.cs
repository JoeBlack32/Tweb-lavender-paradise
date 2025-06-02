using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweb_lavender_paradise.Domain.Entities;
using Tweb_lavender_paradise.Domain.Models;
using Tweb_lavender_paradise.Domain.AutoMapper;


namespace Tweb_lavender_paradise.Domain.DomainHelpers
{
    public static class MappingHelper
    {
        public static OrderHistoryDBTable MapToDb(OrderHistory history)
        {
            return AutoMapperConfig.MapperInstance.Map<OrderHistoryDBTable>(history);
        }

        public static OrderHistory MapToModel(OrderHistoryDBTable historyDb)
        {
            return AutoMapperConfig.MapperInstance.Map<OrderHistory>(historyDb);
        }
    }
}
