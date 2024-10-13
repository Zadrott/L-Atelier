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

        try
        {
            var players = await _playerService.GetPlayersAsync();
            if (players == null || !players.Any())
            {
                _logger.LogWarning("No players found.");
                return NoContent();
            }

            return Ok(players);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving players.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayerById(int id)
    {
        _logger.LogTrace("GetPlayerById request received for ID: {Id}", id);

        try
        {
            var player = await _playerService.GetPlayerByIdAsync(id);

            if (player == null)
            {
                _logger.LogWarning("Player not found with ID: {Id}", id);
                return NotFound(new { Message = $"Player with ID {id} not found." });
            }

            return Ok(player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving player by ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("best_country")]
    public async Task<ActionResult<IEnumerable<CountryStats>>> GetBestCountry()
    {
        _logger.LogTrace("GetBestCountry request received");

        try
        {
            var country = await _playerService.ComputeBestCountryAsync();

            if (country == null)
            {
                _logger.LogWarning("No country stats found.");
                return NoContent();
            }

            return Ok(country);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error computing best country.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("average_imc")]
    public async Task<ActionResult<double>> GetAverageIMC()
    {
        _logger.LogTrace("GetAverageIMC request received");

        try
        {
            var averageIMC = await _playerService.GetAverageIMCAsync();
            return Ok(averageIMC);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating average IMC.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("median_height")]
    public async Task<ActionResult<double>> GetMedianHeight()
    {
        _logger.LogTrace("GetMedianHeight request received");

        try
        {
            var medianHeight = await _playerService.GetMedianHeightAsync();
            return Ok(medianHeight);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating median height.");
            return StatusCode(500, "Internal server error");
        }
    }
}
