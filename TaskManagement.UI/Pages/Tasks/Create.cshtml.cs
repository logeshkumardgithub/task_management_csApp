
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        [BindProperty]
        public TaskItem? Task { get; set; }

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("TaskApi");
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            if (Task != null)
            {
                Task.CreatedDate = DateTime.Now;
            }
            var response = await _httpClient.PostAsJsonAsync("tasks", Task);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Failed to create task.");
            return Page();
        }
    }
}
