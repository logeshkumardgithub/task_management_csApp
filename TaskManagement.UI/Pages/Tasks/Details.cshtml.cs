
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public TaskItem? Task { get; set; }

        public DetailsModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("TaskApi");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Task = await _httpClient.GetFromJsonAsync<TaskItem>($"tasks/{id}");
            return Task == null ? NotFound() : Page();
        }
    }
}
