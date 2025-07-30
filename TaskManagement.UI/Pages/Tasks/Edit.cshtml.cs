
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        [BindProperty]
        public TaskItem Task { get; set; }

        public EditModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("TaskApi");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Task = await _httpClient.GetFromJsonAsync<TaskItem>($"tasks/{id}");
            return Task == null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _httpClient.PutAsJsonAsync($"tasks/{Task.Id}", Task);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Failed to update task.");
            return Page();
        }
    }
}
