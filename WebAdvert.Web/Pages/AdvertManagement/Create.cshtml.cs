using AdvertApi.Models;
using Amazon.CognitoIdentity.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.Web.Models.AdvertManagements;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.ServiceClients.Abstraction;
using WebAdvert.Web.Services.Abstract;

namespace WebAdvert.Web.Pages.AdvertManagement
{
    public class CreateModel : PageModel
    {
        private readonly IFileUploader _fileUploader;
        private readonly IMapper _mapper;
        private readonly IAdvertApiClient _advertApiClient;

        public CreateModel(IFileUploader fileUploader, IMapper mapper, IAdvertApiClient advertApiClient)
        {
            _fileUploader = fileUploader;
            _mapper = mapper;
            _advertApiClient = advertApiClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(CreateAdvertManagementModel requestModel)
        {

            try
            {
                var id = string.Empty;

                var createAdvertModel = _mapper.Map<AdvertModel>(requestModel);

                var responseModel =  await _advertApiClient.Create(advertModel: createAdvertModel);

                id = responseModel.Id;

                var fileName = "";

                if (requestModel.FormFile != null)
                {
                    fileName = Path.GetFileName(requestModel.FormFile.FileName);

                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using (var readStream = requestModel.FormFile.OpenReadStream())
                        {
                            var result = await _fileUploader.FileUploadAsync(filePath, readStream);
                            if (!result)
                                return new JsonResult(false);

                        }


                        ConfirmAdvertModel advertModel = new ConfirmAdvertModel()
                        {
                            Id = id,
                            Status = AdvertStatus.Active
                        };

                        var advertConfirmResponse = await _advertApiClient.Confirm(advertModel);
                        if (!advertConfirmResponse)
                        {
                            return new JsonResult(false);
                        }

                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(false);
                    }
                }
                else
                {
                    return new JsonResult(false);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
        }
    }
}
