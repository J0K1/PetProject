using System.Text.Json.Serialization;

namespace PetProject.Models.Steam
{
    public class SteamPlayerEntity
    {
        [JsonPropertyName("steamid")]
        public string SteamId { get; set; }

        [JsonPropertyName("personaname")]
        public string PersonaName { get; set; }

        [JsonPropertyName("profileurl")]
        public string ProfileUrl { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("avatarmedium")]
        public string AvatarMedium { get; set; }

        [JsonPropertyName("avatarfull")]
        public string AvatarFull { get; set; }

        [JsonPropertyName("personastate")]
        public int PersonaState { get; set; }

        [JsonPropertyName("communityvisibilitystate")]
        public int CommunityVisibilityState { get; set; }

        [JsonPropertyName("profilestate")]
        public int ProfileState { get; set; }

        [JsonPropertyName("lastlogoff")]
        public long LastLogoff { get; set; }

        [JsonPropertyName("commentpermission")]
        public int CommentPermission { get; set; }

        [JsonPropertyName("primaryclanid")]
        public string PrimaryClanId { get; set; }

        [JsonPropertyName("timecreated")]
        public long TimeCreated { get; set; }

        [JsonPropertyName("personastateflags")]
        public int PersonaStateFlags { get; set; }
    }
}
