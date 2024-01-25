using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public JsonResult VerifyCaptcha(string token)
        {
            // Replace with your actual reCAPTCHA Secret Key
            string secretKey = "6LeCm1wpAAAAAJ8cyoIrY2cq7hmbU3lyX_ZKNoQ9";

            // Verify reCAPTCHA v3
            var recaptchaResponse = VerifyRecaptcha(secretKey, token);

            if (recaptchaResponse.Success)
            {
                if (recaptchaResponse.Score >= 0.5) // Adjust the threshold as needed
                {
                    // reCAPTCHA verification succeeded, user is not suspicious
                    return Json(new { success = false, message = "User verification successful." });
                }
                else
                {
                    // reCAPTCHA score is suspicious, return an error
                    return Json(new { success = false, message = "reCAPTCHA score is suspicious." });
                }
            }
            else
            {
                // reCAPTCHA verification failed
                return Json(new { success = false, message = "reCAPTCHA verification failed." });
            }
        }



        private RecaptchaResponse VerifyRecaptcha(string secretKey, string token)
        {
            // Make a POST request to Google's reCAPTCHA verification endpoint
            var client = new System.Net.WebClient();
            var response = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}");

            // Deserialize the response JSON
            var recaptchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RecaptchaResponse>(response);

            return recaptchaResponse;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult> VerifyRecaptchaV2(string token)
        {
            // Replace with your actual reCAPTCHA v2 secret key
            string secretKey = "6LdAqFwpAAAAAFev_QtVHGAgUMogRhMR2OdwIqgG";

            using (HttpClient httpClient = new HttpClient())
            {
                var values = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "secret", secretKey },
                    { "response", token }
                };

                var content = new FormUrlEncodedContent(values);

                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var recaptchaResult = Newtonsoft.Json.JsonConvert.DeserializeObject<RecaptchaResponse>(responseData);

                        if (recaptchaResult != null && recaptchaResult.Success)
                        {
                            // reCAPTCHA v2 verification succeeded
                            // Add your logic here
                            return Json(new { success = true });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    Console.WriteLine(ex.Message);
                }
            }

            // If reCAPTCHA v2 verification fails
            return Json(new { success = false, message = "reCAPTCHA verification failed" });
        }


    }

    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        public double Score { get; set; }
        // Add other fields from the response if needed
    }

    
}