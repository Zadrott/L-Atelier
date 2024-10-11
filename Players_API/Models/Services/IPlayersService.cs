using Players_API.Models.Entities;
using Players_API.Models.DTO;


namespace Players_API.Models.Services;

public interface IPlayerService
{
    Task<List<Player>> GetPlayersAsync();
    Task<Player> GetPlayerByIdAsync(int id);
    Task<CountryStats> ComputeBestCountry();
}