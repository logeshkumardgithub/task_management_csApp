using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public TaskItem? Task { get; set; }

        public DetailsModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return RedirectToPage("/Login");

            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:5000/api/");
            var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            Task = await httpClient.GetFromJsonAsync<TaskItem>($"tasks/{id}");
            return Task == null ? NotFound() : Page();
        }
    }
}
