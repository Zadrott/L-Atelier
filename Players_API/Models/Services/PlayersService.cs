using Players_API.Models.Entities;
using Players_API.Models.Repositories;

namespace Players_API.Models.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<PlayersList> GetPlayersAsync()
    {
        return await _playerRepository.GetAllPlayersAsync();
    }

    public async Task<Player> GetPlayerByIdAsync(int id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id);
    }
}