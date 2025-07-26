using System.Text.Json.Serialization;

namespace PetProject.Steam.Models
{
    public class SteamPlayerAchievementsResponse
    {
        [JsonPropertyName("playerstats")]
        public SteamPlayerAchievementEntity PlayerStats { get; set; }
    }
}
