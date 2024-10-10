using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Players_API.Models.Entities;

namespace Players_API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    const string fileName = "C:/Dev/L'Atelier/Players_API/players.json";

    private readonly JsonSerializerOptions _options;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(ILogger<PlayersController> logger)
    {
        _logger = logger;
        _options = new JsonSerializerOptions { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
    {
        _logger.LogTrace("GetPlayers request received");

        if (!System.IO.File.Exists(fileName))
        {
            return NotFound("Players file not found");
        }

        var jsonData = LoadPlayersFile();

        return Ok(jsonData.Players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers(int id)
    {
        _logger.LogTrace("GetPlayers request received");

        if (!System.IO.File.Exists(fileName))
        {
            return NotFound("Players file not found");
        }

        var jsonData = LoadPlayersFile();
        var player = jsonData.Players.FirstOrDefault(player => player.Id == id);
        
        if (player == null)
        {
            return NotFound($"Player ID not found ({id})");
        }

        return Ok(player);
    }

    private PlayersFile LoadPlayersFile()
    {
        try
        {
            var readText = System.IO.File.ReadAllText(fileName);
            _logger.LogInformation("Json file content : {readText}", readText);

            return JsonSerializer.Deserialize<PlayersFile>(readText, _options);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to read players file : {ex}", ex);
            throw;
        }
    }
}
