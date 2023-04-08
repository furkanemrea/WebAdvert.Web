using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Pages.Accounts
{
    public class ConfirmEmailResponseModel
    {
        public List<string> Errors { get; set; } = new();
        public bool Succeeded { get; set; }
    }
    public class ConfirmModel : PageModel
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoUserPool;
        public ConfirmModel(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cognitoUserPool = cognitoUserPool;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {

            string email = string.Empty;
            string code = string.Empty;
            var responseModel = new ConfirmEmailResponseModel();
            
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                responseModel.Errors.Add("User Not Found");
                return new JsonResult(responseModel);
            }
            else
            {

                var result = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, code, true).ConfigureAwait(false);
                
                if (result.Succeeded)
                {
                    responseModel.Succeeded = true;
                }
                else
                {
                    responseModel.Succeeded = false;    
                    responseModel.Errors = result.Errors.Select(x => x.Description).ToList();
                }
            }
            return new JsonResult(responseModel);


        }
    }
}
