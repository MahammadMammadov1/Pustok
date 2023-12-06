using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Repositories;
using Pustok.Repositories.Implementations;
using Pustok.Repositories.Interfaces;
using Pustok.Services;
using Pustok.Services.Implementations;
using Pustok.Services.Interfaces;

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
builder.Services.AddScoped<IBookTagsRepository, BookTagsRepository>();
builder.Services.AddScoped<IBookImagesRepository, BookImagesRepository>();


builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlServer("Server=MSI;Database=MVC-BB206-Crud1;Trusted_Connection=True");

});



var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();


app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
