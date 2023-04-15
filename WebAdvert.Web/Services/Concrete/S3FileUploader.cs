using Amazon.S3;
using Amazon.S3.Model;
using WebAdvert.Web.Services.Abstract;

namespace WebAdvert.Web.Services.Concrete
{
    public class S3FileUploader:IFileUploader
    {
        private readonly IConfiguration _configuration;

        public S3FileUploader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> FileUploadAsync(string fileName, Stream stream)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("file name must be specified");

            var bucketName = "fea-files";


            using (var client = new AmazonS3Client(awsAccessKeyId: "AKIAYLURDHWIDTU4Z2HA",awsSecretAccessKey: "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"))
            {
                if (stream.Length > 0)
                {
                    if (stream.CanSeek)
                    {
                        stream.Seek(offset: 0, SeekOrigin.Begin);
                    }
                }

                var requestModel = new PutObjectRequest()
                {
                    AutoCloseStream = true,
                    BucketName = bucketName,
                    InputStream = stream,
                    Key = fileName,
                };

                var response = await client.PutObjectAsync(requestModel).ConfigureAwait(false);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
        }
    }
}
