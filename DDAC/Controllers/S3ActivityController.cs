using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DDAC.Controllers
{
    public class S3ActivityController : Controller
    {
        private const string S3BucketName = "abcareddac";

        public IActionResult Index()
        {
            return View();
        }

        //get back keys from appseting json
        private List<string> getKeys()
        {
            List<string> keys = new List<string>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration configure = builder.Build();

            keys.Add(configure["Values:key1"]);
            keys.Add(configure["Values:key2"]);
            keys.Add(configure["Values:key3"]);

            return keys;
        }

        //ActivityUploadImage
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ActivityUploadImage(List<IFormFile> activityImage)
        {
            //connection
            List<string> keys = getKeys();
            AmazonS3Client s3agent = new AmazonS3Client(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);

            //validation
            foreach (var image in activityImage)
            {
                if (image.Length <= 0)
                {
                    return BadRequest("The file" + image.FileName + " is empty, please try again");
                }
                else if (image.Length >= 1048576)
                {
                    return BadRequest("The image size of " + image.FileName + "is over 1MB, please try again");
                }
                else if (image.ContentType.ToLower() != "image/png" && image.ContentType.ToLower() != "image/jpeg" && image.ContentType.ToLower() != "image/gif")
                {
                    return BadRequest("The file of " + image.FileName + "is not a valid file, please try again");
                }

                try
                {
                    PutObjectRequest request = new PutObjectRequest
                    {
                        InputStream = image.OpenReadStream(),
                        BucketName = S3BucketName,
                        Key = "images/" + image.FileName,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    await s3agent.PutObjectAsync(request);
                }
                catch (AmazonS3Exception e)
                {
                    return BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return RedirectToAction("Index", "S3Activity");


        }

    }
}