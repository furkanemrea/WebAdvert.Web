using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoUserPool;

        public AccountsController(CognitoUserPool cognitoUserPool, UserManager<CognitoUser> userManager, SignInManager<CognitoUser> signInManager)
        {
            _cognitoUserPool = cognitoUserPool;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ActionResult> Signup()
        {
            var model = new SignupModel();
            return View(model);
        }
        [HttpPost("signup")]
        public async Task<JsonResult> UserSignup()
        {
            SignupModel signupModel = new();

            if (ModelState.IsValid)
            {
                var user = _cognitoUserPool.GetUser(signupModel.Email);
                if(user.Status != null)
                {
                    ModelState.AddModelError("User Exists", "User with this email already exist");
                    return new JsonResult(signupModel);

                }

                user.Attributes.Add(CognitoAttribute.Name.AttributeName, signupModel.Email);
                var createdUser = await _userManager.CreateAsync(user,signupModel.Password).ConfigureAwait(false);

                if (createdUser.Succeeded)
                {
                    RedirectToAction("Confirm");
                }
            }
            return new JsonResult(signupModel);

        }
        public async Task<ActionResult> Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Confirm(ConfirmModel confirmModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(confirmModel.Email);
                if (user== null)
                {
                    ModelState.AddModelError("Not Found", "User Not Found");
                    return View(confirmModel);

                }
                else
                {
                    var result = await _userManager.ConfirmEmailAsync(user,confirmModel.Code);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                }
            }

            return View(confirmModel);
          
        }

    }
}
