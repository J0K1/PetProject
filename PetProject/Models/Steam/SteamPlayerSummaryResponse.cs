using Azure;
using System.Numerics;

namespace PetProject.Models.Steam
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
