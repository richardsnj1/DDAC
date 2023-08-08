using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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
		[Authorize(Roles = "Teacher, Admin")]
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
                    return BadRequest("The file of " + image.FileName + " is not a valid file, please try again");
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

        public async Task<IActionResult> ShowImage()
        {
            List<string> values = getKeys();
            var awsS3client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

            List<S3Object> images = new List<S3Object>();

            try
            {
                string token = null;
                do
                {
                    //create List object request to the S3
                    ListObjectsRequest request = new ListObjectsRequest
                    {
                        BucketName = S3BucketName
                    };

                    //getting response (images) back from the S3
                    ListObjectsResponse response = await awsS3client.ListObjectsAsync(request).ConfigureAwait(false);
                    images.AddRange(response.S3Objects);
                    token = response.NextMarker;
                } while (token != null);
            }
            catch (AmazonS3Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return View(images);
        }

		[Authorize(Roles = "Teacher, Admin")]
		public async Task<IActionResult> deleteImage(string ImageName)
        {
            //1. add credential for action
            List<string> values = getKeys();
            AmazonS3Client agent = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

            try
            {
                //create a delete request 
                DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                {
                    BucketName = S3BucketName,
                    Key = ImageName
                };

                await agent.DeleteObjectAsync(deleteRequest);
                return RedirectToAction("ShowImage", "S3Activity");
            }
            catch (AmazonS3Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //return RedirectToAction("ShowImage", "S3Activity");
        }

        public async Task<IActionResult> tempImage(string ImageName)
        {
            List<string> values = getKeys();
            AmazonS3Client agent = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = S3BucketName,
                    Key = ImageName,
                    Expires = DateTime.Now.AddMinutes(1)
                };

                ViewBag.tempImage = agent.GetPreSignedURL(request);
                return View();
            }
            catch (AmazonS3Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public async Task<IActionResult> downloadImage(string ImageName)
        {
            List<string> values = getKeys();
            AmazonS3Client agent = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

            Stream downloadImageStream;
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = S3BucketName,
                    Key = ImageName,
                };
                GetObjectResponse response = await agent.GetObjectAsync(request);

                using (var responseStream = response.ResponseStream)
                {
                    downloadImageStream = new MemoryStream();
                    await responseStream.CopyToAsync(downloadImageStream);
                    downloadImageStream.Position = 0;
                }

            }
            catch (AmazonS3Exception e)
            {
                return BadRequest(e.Message);
            }


            string imageFile = Path.GetFileName(ImageName);

            Response.Headers.Add(
                "Content-Disposition", new ContentDisposition
                {
                    FileName = imageFile,
                    Inline = false
                }.ToString()
            );

            return File(downloadImageStream, "image/jpg");

        }


    }

}