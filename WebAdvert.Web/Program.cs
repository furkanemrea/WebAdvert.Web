using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System.Reflection;
using System.Security.AccessControl;
using WebAdvert.Web.Configurations.Mappers;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.ServiceClients.Abstraction;
using WebAdvert.Web.Services.Abstract;
using WebAdvert.Web.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

#region Create Circuit Breaker Pattern

IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPattern()
{
    return HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
}

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempy => TimeSpan.FromSeconds(Math.Pow(2, retryAttempy)));
}

#endregion

builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


builder.Services.AddCognitoIdentity(config =>
{
    config.Password = new Microsoft.AspNetCore.Identity.PasswordOptions
    {
        RequireDigit = false,
        RequiredLength = 6,
        RequiredUniqueChars = 0,
        RequireLowercase = false,
        RequireNonAlphanumeric = false,
        RequireUppercase = false,
    };

});

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Accounts/Login";
});

builder.Services.AddHttpClient<IAdvertApiClient, AdvertApiClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPattern());


builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AdvertProfile)));

builder.Services.AddTransient<IFileUploader, S3FileUploader>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();



