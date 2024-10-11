using Players_API.Models.DTO;
using Players_API.Models.Entities;
using Players_API.Models.Repositories;

namespace Players_API.Models.Services;

public class PlayerService : IPlayerService
{
    private readonly ILogger<PlayerService> _logger;
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(ILogger<PlayerService> logger, IPlayerRepository playerRepository)
    {
        _logger = logger;
        _playerRepository = playerRepository;
    }

    public async Task<List<Player>> GetPlayersAsync()
    {
        var playersList = await _playerRepository.GetAllPlayersAsync();
        return playersList.Players;
    }

    public async Task<Player> GetPlayerByIdAsync(int id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id);
    }

    public async Task<CountryStats> ComputeBestCountry()
    {
        var playersList = await GetPlayersAsync();
        var countries = playersList.GroupBy(player => player.Country.Code).ToList();
        var stats = countries.Select(country =>
        {
            var payersStats = country.Select(player => ComputeGamesStats(player));

            return new CountryStats
            {
                CountryCode = country.Key,
                PlayersStats = payersStats,
                WinRatio = ComputeCountryRatio(payersStats)
            };
        }).OrderByDescending(country => country.WinRatio);

        foreach (var country in stats)
        {
            _logger.LogInformation("country : {country} - ratio : {ratio}", country.CountryCode, country.WinRatio);
        }

        var bestCountry = stats.FirstOrDefault();

        return bestCountry;
    }

    private GamesStats ComputeGamesStats(Player player)
    {
        float numberOfWins = player.Data.Last.Sum(gameResult => gameResult);
        float totalGames = player.Data.Last.Count();
        float ratio = numberOfWins / totalGames;

        _logger.LogDebug("player : {name} - ratio : {ratio}", player.Firstname, ratio);

        var stats = new GamesStats
        {
            TotalGames = totalGames,
            WonGames = numberOfWins,
            LostGames = totalGames - numberOfWins,
            WinRatio = ratio
        };

        return stats;
    }

    private float ComputeCountryRatio(IEnumerable<GamesStats> playersStats)
    {
        float totalGames = playersStats.Sum(player => player.TotalGames);
        float numberOfWins = playersStats.Sum(player => player.WonGames);
        float ratio = numberOfWins / totalGames;

        return ratio;
    }
}