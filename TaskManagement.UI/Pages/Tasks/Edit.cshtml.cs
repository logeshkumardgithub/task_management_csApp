using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public TaskItem? Task { get; set; }

        public EditModel(IHttpClientFactory clientFactory)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return RedirectToPage("/Login");

            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:5000/api/");
            var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var response = await httpClient.PutAsJsonAsync($"tasks/{Task?.Id}", Task);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Failed to update task.");
            return Page();
        }
    }
}
