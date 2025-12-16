using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineCoursePlatform.Client;
using OnlineCoursePlatform.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with API base address
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://localhost:7248/") // Server API URL (match Server launchSettings)
});

// Register API Services
builder.Services.AddScoped<ICourseApiService, CourseApiService>();
builder.Services.AddScoped<IEnrollmentApiService, EnrollmentApiService>();

await builder.Build().RunAsync();
