using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BuoiToi.Services.Uploads
{
    public class UploadService
    {
        private readonly IConfiguration _configuration;


        private CloudinarySetting _cloudinarySetting;

        private Cloudinary _cloudinary;
        public UploadService(IConfiguration configuration) {
            _configuration = configuration;
            _cloudinarySetting = _configuration.GetSection("CloudinarySettings").Get<CloudinarySetting>();
            var account = new Account(_cloudinarySetting.CloudName, _cloudinarySetting.ApiKey, _cloudinarySetting.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };

            return _cloudinary.UploadAsync(uploadParams);
        }
    }
}
