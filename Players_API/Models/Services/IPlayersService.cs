using Players_API.Models.Entities;

namespace Players_API.Models.Services;

public interface IPlayerService
{
    Task<PlayersList> GetPlayersAsync();
    Task<Player> GetPlayerByIdAsync(int id);
}