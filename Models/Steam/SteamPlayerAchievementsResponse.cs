using System.Text.Json.Serialization;

namespace PetProject.Models.Steam
{
    public class SteamPlayerAchievementsResponse
    {
        [JsonPropertyName("playerstats")]
        public SteamPlayerAchievementEntity PlayerStats { get; set; }
    }
}
