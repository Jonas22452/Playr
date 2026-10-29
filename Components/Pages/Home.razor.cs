
using Playr.Models;

namespace Playr.Components.Pages;

public partial class Home
{
    // Time between automatic featured game transitions.
    private const int CarouselIntervalSeconds = 7;

    private List<Game> games = new();
    private Game? selectedGame;
    private Game? featuredGame;
    private Game? previousFeaturedGame;

    private string searchText = string.Empty;
    private string sortBy = "popular";
    private bool homeMenuOpen = true;
    private bool showExtraDetails;
    private int featuredIndex;
    private CancellationTokenSource? carouselCancellation;

    protected override void OnInitialized()
    {
        // Load the baseline recommendations and randomly select the first
        // featured game so the carousel doesn't always start with the same title.
        games = Recommender.GetMostPopular();

        if (games.Count == 0)
        {
            return;
        }

        featuredIndex = Random.Shared.Next(games.Count);
        featuredGame = games[featuredIndex];
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        // Start the carousel only once, after the initial interactive render.
        // A cancellation token allows us to stop the timer when leaving the page.
        if (firstRender && games.Count > 1)
        {
            carouselCancellation = new CancellationTokenSource();
            _ = RunCarouselAsync(carouselCancellation.Token);
        }

        return Task.CompletedTask;
    }

    private async Task RunCarouselAsync(CancellationToken cancellationToken)
    {
        try
        {
            // PeriodicTimer avoids blocking the UI thread while waiting
            // between automatic carousel transitions.
            using var timer = new PeriodicTimer(
                TimeSpan.FromSeconds(CarouselIntervalSeconds));

            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                // Blazor component state must be updated through its
                // synchronization context when called from a background timer.
                await InvokeAsync(() =>
                    ChangeFeatured((featuredIndex + 1) % games.Count));
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Cancellation is expected when the user navigates away.
        }
    }

    private void ToggleHomeMenu() => homeMenuOpen = !homeMenuOpen;

    private void PreviousFeatured() =>
        ChangeFeatured((featuredIndex - 1 + games.Count) % games.Count);

    private void NextFeatured() =>
        ChangeFeatured((featuredIndex + 1) % games.Count);

    private void ChangeFeatured(int index)
    {
        // Ignore invalid indexes and avoid unnecessary rerenders
        // when the requested game is already featured.
        if (index == featuredIndex || index < 0 || index >= games.Count)
        {
            return;
        }

        // Keep the previous game available for the fade-out animation.
        previousFeaturedGame = featuredGame;
        featuredIndex = index;
        featuredGame = games[index];
        StateHasChanged();
    }

    private List<Game> filteredGames
    {
        get
        {
            // Apply filtering and sorting to the current recommendations
            // without modifying the original games collection.
            IEnumerable<Game> result = games;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                // Case-insensitive search allows partial game title matches.
                result = result.Where(game =>
                    game.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            // The featured game is excluded to avoid showing it twice.
            result = result.Where(game => game.AppId != featuredGame?.AppId);

            // Keep sorting separate from filtering so users can change
            // the display order without changing which games are available.
            result = sortBy switch
            {
                "rated" => result.OrderByDescending(GetRating),
                "name" => result.OrderBy(game => game.Name),
                _ => result.OrderByDescending(game => game.Positive + game.Negative)
            };

            return result.ToList();
        }
    }

    private void OpenGame(Game game)
    {
        selectedGame = game;
        showExtraDetails = false;
    }

    private void CloseGame()
    {
        selectedGame = null;
        showExtraDetails = false;
    }

    private void ToggleExtraDetails() => showExtraDetails = !showExtraDetails;

    private static int GetRating(Game game)
    {
        // Calculate the percentage of positive reviews.
        // Return zero when no reviews exist to avoid division by zero.
        int total = game.Positive + game.Negative;
        return total == 0 ? 0 : (int)Math.Round((double)game.Positive / total * 100);
    }

    private static string FormatReviews(int reviews)
    {
        // Shorten large review counts to keep the UI compact.
        // For example: 12500 -> 12.5K and 1500000 -> 1.5M.
        if (reviews >= 1_000_000)
        {
            return $"{reviews / 1_000_000.0:0.0}M";
        }

        if (reviews >= 1_000)
        {
            return $"{reviews / 1_000.0:0.#}K";
        }

        return reviews.ToString();
    }

    private static string GetPrice(Game game)
    {
        if (!game.Price.HasValue)
        {
            return "Unknown";
        }

        return game.Price.Value == 0 ? "Free" : $"€{game.Price.Value:0.00}";
    }

    private static IEnumerable<string> SplitValues(string? values)
    {
        // Steam dataset fields may contain multiple values separated
        // by commas or semicolons. Split them for individual UI badges.
        if (string.IsNullOrWhiteSpace(values))
        {
            return Array.Empty<string>();
        }

        return values.Split(
                new[] { ',', ';' },
                StringSplitOptions.RemoveEmptyEntries)
            .Select(value => value.Trim());
    }

    private static string FirstGenre(string? genres) =>
        SplitValues(genres).FirstOrDefault() ?? "Game";

    public ValueTask DisposeAsync()
    {
        // Stop the background carousel when Blazor disposes this component.
        // This prevents the timer from continuing after navigation.
        carouselCancellation?.Cancel();
        carouselCancellation?.Dispose();
        return ValueTask.CompletedTask;
    }
}
