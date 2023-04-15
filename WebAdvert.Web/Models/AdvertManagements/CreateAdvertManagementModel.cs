namespace WebAdvert.Web.Models.AdvertManagements
{
    public class CreateAdvertManagementModel
    {
        public IFormFile FormFile { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
