using AdvertApi.Models.Response;
using AdvertApi.Models;

namespace WebAdvert.Web.ServiceClients.Abstraction
{
    public interface IAdvertApiClient
    {
        Task<CreateAdvertResponse> Create(AdvertModel advertModel);
        Task<bool> Confirm(ConfirmAdvertModel confirmAdvertModel);

    }
}
