using Messanger.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(MessageMappingProfile));
            services.AddAutoMapper(typeof(RoomMappingProfile));
        }
    }
}
