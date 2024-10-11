using Players_API.Models.Entities;

namespace Players_API.Models.Repositories;

// The repository pattern is intended to abstract the data retreiving logic
// This abstraction will help if we change the data source from JSON file to a real database later

public interface IPlayerRepository
{
    Task<PlayersList> GetAllPlayersAsync();
    Task<Player> GetPlayerByIdAsync(int id);
}