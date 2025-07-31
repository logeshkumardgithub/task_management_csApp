using TaskManagement.API.Data;
using TaskManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManagement.API.Authentication;
using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TasksDb"));
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Task API", Version = "v1" });

    // Add basic auth to swagger
    c.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "basic",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Basic Auth Header"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Tasks.AddRange(
        new TaskManagement.API.Models.TaskItem { Title = "Task 1", Description = "Task 1 data" },
        new TaskManagement.API.Models.TaskItem { Title = "Task 2", Description = "Task 2 data", IsCompleted = true },
        new TaskManagement.API.Models.TaskItem { Title = "Task 3", Description = "Task 3 data", IsCompleted = true },
        new TaskManagement.API.Models.TaskItem { Title = "Task 4", Description = "Task 4 data", IsCompleted = false },
        new TaskManagement.API.Models.TaskItem { Title = "Task 5", Description = "Task 5 data", IsCompleted = true },
        new TaskManagement.API.Models.TaskItem { Title = "Task 6", Description = "Task 6 data", IsCompleted = false },
        new TaskManagement.API.Models.TaskItem { Title = "Task 7", Description = "Task 7 data", IsCompleted = true },
        new TaskManagement.API.Models.TaskItem { Title = "Task 8", Description = "Task 8 data", IsCompleted = true },
        new TaskManagement.API.Models.TaskItem { Title = "Task 9", Description = "Task 9 data", IsCompleted = false },
        new TaskManagement.API.Models.TaskItem { Title = "Task 10", Description = "Task 10 data", IsCompleted = false }
    );

    db.Users.AddRange(
        new AppUser { Username = "admin", Password = "password" },
        new AppUser { Username = "user1", Password = "user1pass" },
        new AppUser { Username = "user2", Password = "user2pass" },
        new AppUser { Username = "user3", Password = "user3pass" }
    );
    db.SaveChanges();
}

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
