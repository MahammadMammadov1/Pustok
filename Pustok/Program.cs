using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;
using Pustok.Core.Models;
using Pustok.DAL;
using Pustok.Repositories;
using Pustok.Repositories.Implementations;
using Pustok.Repositories.Interfaces;
using Pustok.Services;
using Pustok.Services.Implementations;
using Pustok.Services.Interfaces;
using Pustok.ViewService;
using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IBookTagsRepository, BookTagsRepository>();
builder.Services.AddScoped<IBookImagesRepository, BookImagesRepository>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlServer("Server=MSI;Database=MVC-BB206-Crud1;Trusted_Connection=True");

});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredUniqueChars = 0;
    opt.Password.RequiredLength = 8;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireNonAlphanumeric = true;

    opt.User.RequireUniqueEmail = false;
    
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddSignalR();



var app = builder.Build();


app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatUrl");
app.Run();
