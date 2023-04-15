using AdvertApi.Models;
using AutoMapper;
using WebAdvert.Web.Models.AdvertManagements;

namespace WebAdvert.Web.Configurations.Mappers
{
    public class AdvertProfile:Profile
    {
        public AdvertProfile()
        {
            CreateMap<CreateAdvertManagementModel, AdvertModel>();
        }
    }
}
