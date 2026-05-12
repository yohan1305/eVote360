using Evote365.Core.Application;
using Evote365.Core.Application.Interfaces;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Infrastructure.Persistence;
using Evote365.Infrastructure.Persistence.Context;
using Evote365.Infrastructure.Persistence.Seed;
using Evote365.Infrastructure.Shared;
using eVote365_2025;
using eVote365_2025.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Evote365DbContext>(opt =>
    opt.UseSqlServer(connectionString));

builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddPersistenceLayerIoc(builder.Configuration);
builder.Services.AddApplicationLayerIoc();
builder.Services.AddScoped<IUserSession, UserSession>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });

builder.Services.AddAuthorization();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Evote365DbContext>();
    await DatabaseInitializer.SeedAsync(dbContext);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Elector}/{action=VerificarCedula}/{id?}")
    .WithStaticAssets();

app.Run();
