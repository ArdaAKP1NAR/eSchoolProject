using eSchoolDatabase;
using eSchoolProject;
using eSchoolProject.Authorization;
using eSchoolProject.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddServices();
builder.Services.AddDbContext<eSchoolContext>(options => options.UseSqlServer("Server=DESKTOP-4153DFB;Database=eSchoolDatabase;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddMudServices();
builder.Services.ConfigureAuthorizationServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.ConfigureAuthorizationMiddleWare();
app.UseAntiforgery();
app.MapRazorPages();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
