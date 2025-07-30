using System.Text;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages and HttpClient
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("TaskApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/api/");
    var byteArray = Encoding.ASCII.GetBytes("admin:password");
    client.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
