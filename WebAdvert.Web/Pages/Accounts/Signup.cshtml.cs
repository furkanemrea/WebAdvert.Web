using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Pages.Accounts
{
    public class SignupResponseModel
    {
        public List<string> Errors { get; set; } = new();
        public bool Succeeded { get; set; }
        
    }
    public class SignupRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class SignupModel : PageModel
    {


        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoUserPool;
        public SignupModel(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cognitoUserPool = cognitoUserPool;
        }

        public void OnGet()
        
        {

        }
        public async Task<IActionResult> OnPost(SignupRequestModel requestModel)
        {
            SignupResponseModel signupResponseModel = new SignupResponseModel();

            string email = "furkanemrealtintas@gmail.com";
            string password = "123123Fea.";
            string confirmPassword = password;


            if (!string.IsNullOrEmpty(password) && password.Equals(confirmPassword))
            {
                var user = _cognitoUserPool.GetUser(email);
                if (user.Status != null)
                {
                    signupResponseModel.Errors.Add("Email already exist ");
                }

                user.Attributes.Add(CognitoAttribute.Name.AttributeName, email);
                var createdUser = await _userManager.CreateAsync(user, password).ConfigureAwait(false);

                if (createdUser.Succeeded)
                {
                    signupResponseModel.Succeeded = true;
                    return new JsonResult(signupResponseModel);
                }
                else
                {
                    signupResponseModel.Errors = createdUser.Errors.Select(x => x.Description).ToList();
                    signupResponseModel.Succeeded = false;
                    return new JsonResult(signupResponseModel);

                }
            }
            else
                signupResponseModel.Errors.Add("please check your password or email informations ! ");
    
            return new JsonResult(signupResponseModel);
        }

        
    }
}
