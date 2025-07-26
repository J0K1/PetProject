using System.Net;

namespace PetProject.Steam.Services
{
    public class SteamService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public SteamService(string apiKey, HttpClient httpClient)
        {
            _apiKey = apiKey;
            _httpClient = httpClient;
        }

        public async Task<string> GetPlayerSummary(string steamId)
        {
            var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={_apiKey}&steamids={steamId}"; 
            var response = await _httpClient.GetAsync(url);

            if(response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new Exception("Steam API rate limit exceeded. Try again later.");
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPlayerAchievements(string steamId, string appId)
        {
            var url = $"http://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v1/?appid={appId}&key={_apiKey}&steamid={steamId}&l=russian";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new Exception("Steam API rate limit exceeded. Try again later.");
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
