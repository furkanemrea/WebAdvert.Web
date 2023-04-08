using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts
{
    public class ConfirmModel
    {
        public string Email { get; set; }

        public string Code { get; set; } 
    }
}
