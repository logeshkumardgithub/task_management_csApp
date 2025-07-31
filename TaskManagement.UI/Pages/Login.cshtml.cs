using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;

namespace TaskManagement.UI.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var byteArray = Encoding.ASCII.GetBytes($"{Username}:{Password}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            try
            {
                // Try calling any endpoint just to validate
                var response = await client.GetAsync("http://localhost:5000/api/tasks");

                if (response.IsSuccessStatusCode)
                {
                    // Store credentials in session
                    HttpContext.Session.SetString("Username", Username);
                    HttpContext.Session.SetString("Password", Password);
                    return RedirectToPage("/Tasks/Index");
                }
                else
                {
                    ErrorMessage = "Invalid username or password.";
                    return Page();
                }
            }
            catch
            {
                ErrorMessage = "Unable to connect to the API.";
                return Page();
            }
        }
    }
}
