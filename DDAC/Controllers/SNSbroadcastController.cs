using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DDAC.Controllers
{
    public class SNSbroadcastController : Controller
    {
        private const string topicARN = "arn:aws:sns:us-east-1:606895753313:ABCareNotification";

        //get keys from appsettings.json
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
        
        //subscribe newsletter (for customer)
        public IActionResult Index()
        {
            return View();
        }

        //process the subscription
        public async Task<IActionResult> processSubscription(string email)
        {
            List<string> keys = getKeys();
            AmazonSimpleNotificationServiceClient agent =
                new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            try
            {
                SubscribeRequest request = new SubscribeRequest
                {
                    TopicArn = topicARN,
                    Protocol = "Email",
                    Endpoint = email
                };
                SubscribeResponse response = await agent.SubscribeAsync(request);
                ViewBag.subscriptionSuccessID = response.ResponseMetadata.RequestId;
                return View();
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //broadcast form
        public IActionResult BroadCastMessage()
        {
            return View();
        }

        //broadcast
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> processBroadcast(string subjecttitle, string broadcastText)
        {
            List<string> keys = getKeys();
            var snsClient = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);

            if (ModelState.IsValid)
            {
                try
                {
                    //add email as the subscriber
                    PublishRequest pubRequest = new PublishRequest
                    {
                        TopicArn = topicARN,
                        Subject = subjecttitle,
                        Message = broadcastText
                    };
                    PublishResponse pubResponse = await snsClient.PublishAsync(pubRequest);
                    // Store success message in TempData
                    TempData["emailsent"] = "Email sent out to your customer";

                    // Redirect to a different page
                    return RedirectToAction("Index", "Home"); // Replace "Index" and "Home" with the desired action and controller names
                }
                catch (AmazonSimpleNotificationServiceException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Unable to broadcast message to any email!");
            }
        }
    }
}
