namespace PetProject.Shared.Constants
{
    public static class CacheKeys
    {
        public static string GameId(int id) => $"game:{id}";

        public static string AllGames(int? year, string? genre) 
            => $"games:year={(year?.ToString() ?? "all")}::genre={(string.IsNullOrWhiteSpace(genre) ? "all" : genre.Trim().ToLower())}";

        public const string AllGenres = "genres:all";

    }
}
