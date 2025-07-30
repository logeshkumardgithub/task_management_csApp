using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<TaskItem> Tasks { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; } // "Completed", "Pending"

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("TaskApi");
        }

        public async Task OnGetAsync()
        {
            try
            {
                var title = string.IsNullOrWhiteSpace(SearchTitle) ? "" : $"title={Uri.EscapeDataString(SearchTitle)}";
                var status = string.IsNullOrWhiteSpace(FilterStatus) ? "" : $"status={Uri.EscapeDataString(FilterStatus)}";

                var query = string.Join("&", new[] { title, status }.Where(q => !string.IsNullOrEmpty(q)));

                var requestUri = string.IsNullOrEmpty(query) ? "tasks" : $"tasks?{query}";

                Tasks = await _httpClient.GetFromJsonAsync<List<TaskItem>>(requestUri) ?? new List<TaskItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch tasks: {ex.Message}");
                Tasks = new List<TaskItem>();
            }
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"tasks/{id}");
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
