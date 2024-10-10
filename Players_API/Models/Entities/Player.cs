namespace Players_API.Models.Entities;

public record PlayersFile
{
    public required IEnumerable<Player> Players { get; set; }
}

public record Player
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Shortname { get; set; }
    public string? Sex { get; set; }
    public Country? Country { get; set; }
    public string? Picture { get; set; }
    public PlayerData? Data { get; set; }
}

public enum Gender
{
    M,
    F
}

public record Country
{
    public string? Picture { get; set; }
    public string? Code { get; set; }
}

public record PlayerData
{
    public int Rank { get; set; }
    public int Points { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int Age { get; set; }
    public int[]? Last { get; set; }
}