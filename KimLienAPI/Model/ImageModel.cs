namespace KimLienAPI.Model
{
    public class ImageModel
    {
        [Validation.AllowedExtensions(".jpg",".png",".jpeg")]
        public IFormFile Image { get; set; } = null!;
    }
}
