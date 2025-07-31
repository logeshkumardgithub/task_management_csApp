using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TaskManagement.UI.Models;

namespace TaskManagement.UI.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public TaskItem Task { get; set; } = new TaskItem();

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToPage("/Login");

            return Page();
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

            Task.CreatedDate = DateTime.Now;
            var response = await httpClient.PostAsJsonAsync("tasks", Task);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Failed to create task.");
            return Page();
        }
    }
}
