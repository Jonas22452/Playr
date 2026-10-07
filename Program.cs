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

            using (var scope = app.Services.CreateScope())
            {
                var recommender = scope.ServiceProvider.GetRequiredService<BasicRecommender>();

                var games = recommender.GetMostPopular();

                foreach (var game in games)
                {
                    Console.WriteLine($"{game.Name} - {game.AveragePlaytime}");
                }
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

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
