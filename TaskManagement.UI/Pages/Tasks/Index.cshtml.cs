using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public List<TaskItem> Tasks { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; }

        [BindProperty(SupportsGet = false)]
        public string? ErrorMessage { get; set; }

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve credentials from TempData
            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                // Redirect to login if not authenticated
                return RedirectToPage("/Login");
            }

            try
            {
                var httpClient = _clientFactory.CreateClient();
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                httpClient.BaseAddress = new Uri("http://localhost:5000/api/");

                var title = string.IsNullOrWhiteSpace(SearchTitle) ? "" : $"title={Uri.EscapeDataString(SearchTitle)}";
                var status = string.IsNullOrWhiteSpace(FilterStatus) ? "" : $"status={Uri.EscapeDataString(FilterStatus)}";

                var query = string.Join("&", new[] { title, status }.Where(q => !string.IsNullOrEmpty(q)));
                var requestUri = string.IsNullOrEmpty(query) ? "tasks" : $"tasks?{query}";

                Tasks = await httpClient.GetFromJsonAsync<List<TaskItem>>(requestUri) ?? new List<TaskItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch tasks: {ex.Message}");
                ErrorMessage = "Authentication failed or API unreachable.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var httpClient = _clientFactory.CreateClient();
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                httpClient.BaseAddress = new Uri("http://localhost:5000/api/");

                var response = await httpClient.DeleteAsync($"tasks/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage(); // Refresh page to reload tasks
                }

                ModelState.AddModelError(string.Empty, "Failed to delete the task.");
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting task: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the task.");
                return Page();
            }
        }
    }
}
