using SteamWebAPI2.Models.SteamStore;
using System.Text.Json.Serialization;

namespace PetProject.Steam.Models
{
    public class SteamPlayerAchievementEntity
    {
        [JsonPropertyName("steamID")]
        public string SteamId { get; set; }

        [JsonPropertyName("gameName")]
        public string GameName { get; set; }

        [JsonPropertyName("achievements")]
        public List<Achievement> Achievements { get; set; }

    }

    public class Achievement
    {
        [JsonPropertyName("apiname")]
        public string ApiName { get; set; }

        [JsonPropertyName("achieved")]
        public int Achieved { get; set; }

        [JsonPropertyName("unlocktime")]
        public long UnlockTime { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
