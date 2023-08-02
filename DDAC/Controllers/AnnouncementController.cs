using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace DDAC.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly HttpClient _httpClient;

        public AnnouncementController()
        {
            _httpClient = new HttpClient();
        }

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
        public async Task<ActionResult> Announce()
        {
            string apiUrl = "https://etxn0affei.execute-api.us-east-1.amazonaws.com/Development";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Inform the user of success (you can customize the message)
                    ViewBag.Message = "Email Sent!";
                }
                else
                {
                    // Handle error response or show an appropriate message
                    ViewBag.Message = "Email Failed to Sent!";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ViewBag.Message = "An error occurred: " + ex.Message;
            }

            // Render the view with the message
            return View();
        }
    }
}
