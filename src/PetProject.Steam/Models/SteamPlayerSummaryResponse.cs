namespace PetProject.Steam.Models
{
    public class SteamPlayerSummaryResponse
    {
        public Response Response { get; set; }
    }

    public class Response
    {
        public List<SteamPlayerEntity> Players { get; set; }
    }
}
