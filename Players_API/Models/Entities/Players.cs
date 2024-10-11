namespace Players_API.Models.Entities;

// Using records instead of classes ensure immutability and value-based equality
// Also helps to keep the model simple and concise as the computed values and business logic are handled by the Player Service

public record PlayersList
{
    public required List<Player> Players { get; set; }
}

public record Player
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Shortname { get; set; }
    public string? Sex { get; set; }
    public required Country Country { get; set; }
    public string? Picture { get; set; }
    public required PlayerData Data { get; set; }
}

public enum Gender
{
    M,
    F
}

public record Country
{
    public string? Picture { get; set; }
    public required string Code { get; set; }
}

public record PlayerData
{
    public int Rank { get; set; }
    public int Points { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int Age { get; set; }
    public required int[] Last { get; set; }
}
