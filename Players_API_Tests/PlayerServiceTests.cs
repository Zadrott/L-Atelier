using Microsoft.Extensions.Logging;
using Moq;
using Players_API.Models.Entities;
using Players_API.Models.Repositories;
using Players_API.Models.Services;

namespace Players_API.Tests
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _playerService = new PlayerService(new Mock<ILogger<PlayerService>>().Object, _mockPlayerRepository.Object);
        }

        [Fact]
        public async Task GetAverageIMCAsync_ShouldReturnCorrectAverageIMC()
        {
            var players = new List<Player>
            {
                new Player
                {
                    Id = 1,
                    Firstname = "Player 1",
                    Shortname = "P1",
                    Country = new Country { Code = "US" },
                    Data = new PlayerData
                    {
                        Weight = 70000,
                        Height = 180,
                        Last = [1, 0, 1]
                    }
                },
                new Player
                {
                    Id = 2,
                    Firstname = "Player 2",
                    Shortname = "P2",
                    Country = new Country { Code = "US" },
                    Data = new PlayerData
                    {
                        Weight = 80000,
                        Height = 170,
                        Last = [0, 1, 1]
                    }
                }
            };

            _mockPlayerRepository.Setup(repo => repo.GetAllPlayersAsync())
                                 .ReturnsAsync(new PlayersList { Players = players });

            var result = await _playerService.GetAverageIMCAsync();

            // Calculate expected average IMC manually
            float expectedIMC1 = 70 / (1.8f * 1.8f);
            float expectedIMC2 = 80 / (1.7f * 1.7f);
            float expectedAverageIMC = (expectedIMC1 + expectedIMC2) / 2;

            // Assert: Verify that the calculated average IMC matches the expected value
            Assert.Equal(expectedAverageIMC, result, 2);  // 2 decimal precision
        }

        [Fact]
        public async Task GetMedianHeightAsync_ShouldReturnCorrectMedianHeightWithOddPlayersNumber()
        {
            // Arrange: Create mock player data with varying heights and valid Country and Shortname
            var players = new List<Player>
            {
                new Player { Id = 1, Shortname = "P1", Country = new Country { Code = "US" }, Data = new PlayerData { Height = 180, Last = [1, 0, 1] } },
                new Player { Id = 2, Shortname = "P2", Country = new Country { Code = "US" }, Data = new PlayerData { Height = 175, Last = [1, 0, 1] } },
                new Player { Id = 3, Shortname = "P3", Country = new Country { Code = "FR" }, Data = new PlayerData { Height = 160, Last = [1, 0, 1] } },
                new Player { Id = 4, Shortname = "P4", Country = new Country { Code = "FR" }, Data = new PlayerData { Height = 170, Last = [1, 0, 1] } },
                new Player { Id = 5, Shortname = "P5", Country = new Country { Code = "CA" }, Data = new PlayerData { Height = 190, Last = [1, 0, 1] } }
            };

            _mockPlayerRepository.Setup(repo => repo.GetAllPlayersAsync())
                                 .ReturnsAsync(new PlayersList { Players = players });

            var result = await _playerService.GetMedianHeightAsync();

            // The expected median of the heights [160, 170, 175, 180, 190] is 175
            float expectedMedian = 175;

            // Assert: Verify that the calculated median height matches the expected value
            Assert.Equal(expectedMedian, result);
        }

        [Fact]
        public async Task GetMedianHeightAsync_ShouldReturnCorrectMedianHeightWithEvenPlayersNumber()
        {
            // Arrange: Create mock player data with varying heights and valid Country and Shortname
            var players = new List<Player>
            {
                new Player { Id = 1, Shortname = "P1", Country = new Country { Code = "US" }, Data = new PlayerData { Height = 180, Last = [1, 0, 1] } },
                new Player { Id = 2, Shortname = "P2", Country = new Country { Code = "US" }, Data = new PlayerData { Height = 175, Last = [1, 0, 1] } },
                new Player { Id = 3, Shortname = "P3", Country = new Country { Code = "FR" }, Data = new PlayerData { Height = 160, Last = [1, 0, 1] } },
                new Player { Id = 4, Shortname = "P4", Country = new Country { Code = "FR" }, Data = new PlayerData { Height = 170, Last = [1, 0, 1] } }
            };

            _mockPlayerRepository.Setup(repo => repo.GetAllPlayersAsync())
                                 .ReturnsAsync(new PlayersList { Players = players });

            var result = await _playerService.GetMedianHeightAsync();

            // The expected median of the heights [160, 170, 175, 180] is the average of 170 and 175
            float expectedMedian = (170 + 175) / 2;

            // Assert: Verify that the calculated median height matches the expected value
            Assert.Equal(expectedMedian, result);
        }

        [Fact]
        public async Task ComputeBestCountryAsync_ReturnsCountryWithHighestWinRatio()
        {
            var playersList = new PlayersList
            {
                Players = new List<Player> {
                    new Player
                    {
                        Id = 1,
                        Shortname = "P1",
                        Country = new Country { Code = "US" },
                        Data = new PlayerData { Last = [1, 1, 0] }
                    },
                    new Player
                    {
                        Id = 2,
                        Shortname = "P2",
                        Country = new Country { Code = "US" },
                        Data = new PlayerData { Last = [1, 0, 0] }
                    },
                    new Player
                    {
                        Id = 3,
                        Shortname = "P3",
                        Country = new Country { Code = "FR" },
                        Data = new PlayerData { Last = [1, 1, 1] }
                    }
                }
            };

            _mockPlayerRepository.Setup(repo => repo.GetAllPlayersAsync()).ReturnsAsync(playersList);

            // Act
            var result = await _playerService.ComputeBestCountryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("FR", result.CountryCode);
            Assert.Equal(1.0, result.WinRatio, 1);
        }
    }
}
