using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.AdvertManagements;
using WebAdvert.Web.ServiceClients.Abstraction;
using WebAdvert.Web.Services.Abstract;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagementController:Controller
    {

        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public AdvertManagementController(IFileUploader fileUploader, IMapper mapper, IAdvertApiClient advertApiClient)
        {
            _fileUploader = fileUploader;
            _mapper = mapper;
            _advertApiClient = advertApiClient;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost("create")]
        public async Task<bool> CreateAdvert()
        {
            return true;
        }
    }
}
