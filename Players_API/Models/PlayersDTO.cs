using Players_API.Models.Entities;

namespace Players_API.Models.DTO;

public record PlayerDto : Player
{
    public GamesStats? Stats { get; set; }
}

public record GamesStats
{
    public float TotalGames { get; set; }
    public float WonGames { get; set; }
    public float LostGames { get; set; }
    public float WinRatio { get; set; }
}

public record CountryStats
{
    public required string CountryCode { get; set; }
    public required IEnumerable<GamesStats> PlayersStats { get; set; }
    public required float WinRatio { get; set; }
}
