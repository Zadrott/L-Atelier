using Players_API.Models.Entities;

namespace Players_API.Models.DTO;

public record PlayerDto
{
    // Properties derived from Player entity
    public int Id { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public required string Shortname { get; init; }
    public Gender? Sex { get; init; }
    public string? Picture { get; init; }
    public required Country Country { get; init; }
    // Computed properties
    public required PlayerDataDto Data { get; init; }    

     // Constructor that takes a Player entity, GamesStats, and IMC
    public PlayerDto(Player player, GamesStats gamesStats, float imc)
    {
        Id = player.Id;
        Firstname = player.Firstname;
        Lastname = player.Lastname;
        Shortname = player.Shortname;
        Sex = player.Sex;
        Picture = player.Picture;
        Country = player.Country;
        Data = new PlayerDataDto(player.Data, gamesStats, imc);
    }
}

public record PlayerDataDto
{
    public int Rank { get; init; }
    public int Points { get; init; }
    public int Weight { get; init; }
    public int Height { get; init; }
    public int Age { get; init; }
    public float IMC { get; init; }
    public int[] Last { get; init; }
    public GamesStats? Stats { get; init; }

    public PlayerDataDto(PlayerData data, GamesStats stats, float imc)
    {
        Rank = data.Rank;
        Points = data.Points;
        Weight = data.Weight;
        Height = data.Height;
        Age = data.Age;
        Last = data.Last;
        Stats = stats;
        IMC = imc;
    }
}

public record CountryStats
{
    public required string CountryCode { get; init; }
    public required IEnumerable<GamesStats> PlayersStats { get; init; }
    public required float WinRatio { get; init; }
}

public record GamesStats
{
    public float TotalGames { get; init; }
    public float WonGames { get; init; }
    public float LostGames { get; init; }
    public float WinRatio { get; init; }
}
