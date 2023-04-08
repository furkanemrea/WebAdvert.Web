using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;

namespace WebAdvert.Web.Pages.Accounts
{
    public class LoginResponseModel
    {
        public List<string> Errors { get; set; } = new(1);
        public bool Succeeded { get; set; }
    }
    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class LoginModel : PageModel
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;


        public LoginModel(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public async Task<IActionResult> OnPost(LoginRequestModel loginRequestModel)
        {

            LoginResponseModel loginResponseModel = new LoginResponseModel();
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequestModel.Email);


                if (user == null)
                {
                    loginResponseModel.Succeeded = false;
                    loginResponseModel.Errors.Add("Incorrect data ! ");
                }
                else
                {

                    var infoCheckResponse = await _signInManager.PasswordSignInAsync(loginRequestModel.Email, loginRequestModel.Password, loginRequestModel.RememberMe, false);

                    if (infoCheckResponse.Succeeded)
                    {
                        loginResponseModel.Succeeded = true;
                    }
                    else
                    {
                        loginResponseModel.Succeeded = false;
                        loginResponseModel.Errors.Add("Incorrect data ! ");
                    }

                }

            }
            catch (Exception ex)
            {
                loginResponseModel.Errors.Add(ex.Message);
            }


            return new JsonResult(loginResponseModel);
        }
    }
}
