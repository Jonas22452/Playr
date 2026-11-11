using Playr.Components;
using Playr.Repositories;
using Playr.Services;

namespace Playr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddSingleton<GameRepository>();
            builder.Services.AddSingleton<BasicRecommender>();

            var app = builder.Build();

            // Check at startup that the local CSV is mounted and readable.
            app.Services.GetRequiredService<GameRepository>();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapGet("/api/recommendations", (BasicRecommender recommender) =>
            {
                return recommender.GetMostPopular();
            });

            app.Run();
        }
    }
}
