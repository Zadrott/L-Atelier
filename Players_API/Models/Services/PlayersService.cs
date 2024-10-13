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
        //TODO : Use PlayerDTO
        return playersList.Players;
    }

    public async Task<Player> GetPlayerByIdAsync(int id)
    {
        //TODO : Use PlayerDTO
        return await _playerRepository.GetPlayerByIdAsync(id);
    }

    public async Task<CountryStats> ComputeBestCountryAsync()
    {
        var playersList = await GetPlayersAsync();
        var countries = playersList.GroupBy(player => player.Country.Code).ToList();

        var stats = countries.Select(country =>
        {
            var playersStats = country.Select(player => ComputeGamesStats(player));

            return new CountryStats
            {
                CountryCode = country.Key,
                PlayersStats = playersStats,
                WinRatio = ComputeCountryRatio(playersStats)
            };
        }).OrderByDescending(country => country.WinRatio).ToList();

        foreach (var country in stats)
        {
            _logger.LogInformation("Country: {country} - Win Ratio: {ratio}", country.CountryCode, country.WinRatio);
        }

        return stats.FirstOrDefault();
    }

    public async Task<float> GetAverageIMCAsync()
    {
        var players = await GetPlayersAsync();
        var imcSum = players.Sum(player => ComputeIMC(player.Data.Weight, player.Data.Height));
        return imcSum / players.Count;
    }

    public async Task<float> GetMedianHeightAsync()
    {
        var players = await GetPlayersAsync();
        var heights = players.Select(player => player.Data.Height).ToList();
        heights.Sort();

        int count = heights.Count;
        if (count % 2 == 1)
        {
            // If the number of players is odd the median is in the middle of the list
            return heights[count / 2];
        }
        else
        {
            // If the number of players is even the median is in the average of two values in the middle of the list
            return (heights[(count / 2) - 1] + heights[count / 2]) / 2;
        }
    }

    private GamesStats ComputeGamesStats(Player player)
    {
        float totalGames = player.Data.Last.Length;

        if (totalGames == 0)
        {
            // If there are no games, set default stats
            return new GamesStats
            {
                TotalGames = 0,
                WonGames = 0,
                LostGames = 0,
                WinRatio = 0
            };
        }

        float numberOfWins = player.Data.Last.Count(gameResult => gameResult == 1);
        float winRatio = numberOfWins / totalGames;

        _logger.LogDebug("Player: {name}, Win Ratio: {ratio}", player.Firstname, winRatio);

        return new GamesStats
        {
            TotalGames = totalGames,
            WonGames = numberOfWins,
            LostGames = totalGames - numberOfWins,
            WinRatio = winRatio
        };
    }

    private static float ComputeCountryRatio(IEnumerable<GamesStats> playersStats)
    {
        float totalGames = playersStats.Sum(player => player.TotalGames);

        // Handle the case where totalGames might be 0 to avoid division by zero and useless computations
        if (totalGames == 0)
        {
            return 0;
        }

        float numberOfWins = playersStats.Sum(player => player.WonGames);
        return numberOfWins / totalGames;
    }

    private float ComputeIMC(int weightG, int heightCm)
    {
        if (heightCm <= 0 || weightG <= 0)
        {
            _logger.LogError("Weight and height must be positive values.");
            throw new ArgumentException("Weight and height must be positive values.");
        }

        float heightM = heightCm / 100f;
        float weightKg = weightG / 1000f;
        float imc = weightKg / (heightM * heightM);

        _logger.LogDebug("IMC computed: {IMC} for weight: {Weight} and height: {HeightCm}", imc, weightKg, heightM);

        return imc;
    }
}
