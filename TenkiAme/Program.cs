using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TenkiAme.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        //builder.Services.AddControllersWithViews();
        //builder.Services.AddDbContext<TenkiAmeContext>(options =>
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("TenkiAmeContext") ?? throw new InvalidOperationException("Connection string 'TenkiAmeContext' not found.")));

        //Set up dependency injection for using secrets
        //TenkiAme.ServiceConfiguration.ConfigureServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.MapControllers();

        app.UseAuthorization();

        app.Run();
    }
}