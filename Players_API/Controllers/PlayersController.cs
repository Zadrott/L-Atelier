using Microsoft.AspNetCore.Mvc;
using Players_API.Models.DTO;
using Players_API.Models.Services;

namespace Players_API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    private readonly IPlayerService _playerService;

    public PlayersController(ILogger<PlayersController> logger, IPlayerService playerService)
    {
        _logger = logger;
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAllPlayers()
    {
        _logger.LogTrace("GetPlayers request received");

        var players = await _playerService.GetPlayersAsync();
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayerById(int id)
    {
        _logger.LogTrace("GetPlayerById request received");

        var player = await _playerService.GetPlayerByIdAsync(id);

        if (player == null)
        {
            return NotFound($"Player ID not found ({id})");
        }

        return Ok(player);
    }


    [HttpGet("best_country")]
    public async Task<ActionResult<IEnumerable<CountryStats>>> GetBestCountry()
    {
        _logger.LogTrace("GetBestCountry request received");

        var country = await _playerService.ComputeBestCountry();

        return Ok(country);
    }
}
