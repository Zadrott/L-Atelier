using System.Text.Json;
using Players_API.Models.Entities;

namespace Players_API.Models.Repositories;

public class PlayerJsonRepository : IPlayerRepository
{
    private readonly string _fileName;
    private readonly JsonSerializerOptions _options;

    public PlayerJsonRepository(IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentException("Failed to load PlayerJsonRepository configuration");
        }

        _fileName = configuration["PlayersFilePath"] ?? "";

        if (string.IsNullOrWhiteSpace(_fileName))
        {
            throw new ArgumentException("PlayersFilePath must be set in the configuration file");
        }

        _options = new JsonSerializerOptions { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };
    }

    public async Task<PlayersList> GetAllPlayersAsync()
    {
        if (!File.Exists(_fileName))
        {
            throw new FileNotFoundException("Players file not found");
        }

        var jsonContent = await File.ReadAllTextAsync(_fileName);
        var playerList = JsonSerializer.Deserialize<PlayersList>(jsonContent, _options);

        if (playerList == null)
        {
            throw new JsonException("Failed to parse players list");
        }

        return playerList;
    }

    public async Task<Player> GetPlayerByIdAsync(int id)
    {
        var playersList = await GetAllPlayersAsync();
        var player = playersList.Players.FirstOrDefault(player => player.Id == id);

        return player;
    }
}