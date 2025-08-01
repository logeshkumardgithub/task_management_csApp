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

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool SortAsc { get; set; } = true;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 5; // You can change this default value

        [BindProperty(SupportsGet = false)]
        public string? ErrorMessage { get; set; }

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return RedirectToPage("/Login");

            try
            {
                var httpClient = _clientFactory.CreateClient();
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                httpClient.BaseAddress = new Uri("http://localhost:5000/api/");

                var queryParams = new List<string>();
                if (!string.IsNullOrWhiteSpace(SearchTitle))
                    queryParams.Add($"title={Uri.EscapeDataString(SearchTitle)}");

                if (!string.IsNullOrWhiteSpace(FilterStatus))
                    queryParams.Add($"status={Uri.EscapeDataString(FilterStatus)}");

                if (!string.IsNullOrWhiteSpace(SortBy))
                    queryParams.Add($"sortBy={SortBy}");

                queryParams.Add($"sortAsc={SortAsc}");
                queryParams.Add($"pageNumber={PageNumber}");
                queryParams.Add($"pageSize={PageSize}");

                var query = string.Join("&", queryParams);
                var requestUri = $"tasks?{query}";

                var response = await httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.Contains("X-Total-Count"))
                    {
                        var totalCountHeader = response.Headers.GetValues("X-Total-Count").FirstOrDefault();
                        if (int.TryParse(totalCountHeader, out int totalCount))
                        {
                            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                        }
                    }

                    Tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>() ?? new List<TaskItem>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ErrorMessage = "Authentication failed.";
                }
                else
                {
                    ErrorMessage = $"Error fetching tasks: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch tasks: {ex.Message}");
                ErrorMessage = "Failed to connect to API.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return RedirectToPage("/Login");

            try
            {
                var httpClient = _clientFactory.CreateClient();
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                httpClient.BaseAddress = new Uri("http://localhost:5000/api/");

                var response = await httpClient.DeleteAsync($"tasks/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToPage();

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
