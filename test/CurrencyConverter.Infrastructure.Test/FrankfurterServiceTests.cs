using CurrencyConverter.Common.Exceptions;
using CurrencyConverter.Common.Models;
using CurrencyConverter.Frankfurter;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace CurrencyConverter
{
    public class FrankfurterServiceTests
    {
        private readonly Mock<ILogger<FrankfurterService>> _loggerMock;
        private readonly Mock<IFrankfurterAPI> _frankfurterAPIMock;
        private readonly FrankfurterService _service;

        public FrankfurterServiceTests()
        {
            _loggerMock = new Mock<ILogger<FrankfurterService>>();
            _frankfurterAPIMock = new Mock<IFrankfurterAPI>();
            _service = new FrankfurterService(_loggerMock.Object, _frankfurterAPIMock.Object);
        }

        [Fact]
        public async Task GetLatestExchangeAsync_ShouldReturnExchangeRates()
        {
            // Arrange
            var expectedRates = new ExchangeRates
            {
                Amount = 1,
                Base = "USD",
                Date = DateOnly.Parse("2024-08-28"),
                Rates = new(){
                    { "AUD",1.4758m },
                    { "EUR", 0.89952m },
                    { "GBP", 0.75707m },
                    { "HUF", 353.87m },
                    { "IDR", 15427m }
                }
            };

            _frankfurterAPIMock
                .Setup(api => api.GetLatestAsync(It.IsAny<string>(), null, null))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedRates))
                });

            // Act
            var result = await _service.GetLatestExchangeAsync("USD");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedRates);
        }

        [Fact]
        public async Task ConvertAmountAsync_ShouldReturnConvertedExchangeRates()
        {
            // Arrange
            var expectedRates = new ExchangeRates
            {
                Amount = 100,
                Base = "USD",
                Date = DateOnly.Parse("2024-08-28"),
                Rates = new(){
                    { "AUD",147.58m },
                    { "EUR", 89.952m },
                    { "GBP", 75.707m },
                    { "HUF", 35387m },
                    { "IDR", 1542700m }
                }
            };

            _frankfurterAPIMock
                .Setup(api => api.GetLatestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedRates))
                });

            // Act
            var result = await _service.ConvertAmountAsync("USD", "EUR", 100);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedRates);
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_ShouldReturnHistoricalRates()
        {
            // Arrange
            var expectedHistoricalRates = new HistoricalRates
            {
                Amount = 1,
                Base = "USD",
                StartDate = DateOnly.Parse("2024-08-27"),
                EndDate = DateOnly.Parse("2024-08-28"),
                Rates = new(){
                    {
                        DateOnly.Parse("2024-08-27"),
                         new (){
                            { "AUD",147.58m },
                            { "EUR", 89.952m },
                            { "GBP", 75.707m },
                            { "HUF", 35387m },
                            { "IDR", 1542700m }
                        }
                    }
                }
            };
            _frankfurterAPIMock
                .Setup(api => api.GetHistoricalsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedHistoricalRates))
                });

            // Act
            var result = await _service.GetHistoricalRatesAsync(DateOnly.FromDateTime(DateTime.Now), null, "USD", "EUR", 100);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedHistoricalRates);
        }

        [Fact]
        public async Task GetLatestExchangeAsync_ShouldThrowApiException_OnNonSuccessStatusCode()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = new StringContent(JsonSerializer.Serialize(new { message = "Bad Request" })),
            };
            _frankfurterAPIMock
                .Setup(api => api.GetLatestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>()))
                .ReturnsAsync(responseMessage);

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _service.GetLatestExchangeAsync("USD"));
        }

        [Fact]
        public async Task ConvertAmountAsync_ShouldThrowApiException_OnNonSuccessStatusCode()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = new StringContent(JsonSerializer.Serialize(new { message = "Bad Request" })),
            };
            _frankfurterAPIMock
                .Setup(api => api.GetLatestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>()))
                .ReturnsAsync(responseMessage);

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _service.ConvertAmountAsync("USD", "EUR", 100));
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_ShouldThrowApiException_OnNonSuccessStatusCode()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = new StringContent(JsonSerializer.Serialize(new { message = "Bad Request" })),
            };
            _frankfurterAPIMock
                .Setup(api => api.GetHistoricalsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>()))
                .ReturnsAsync(responseMessage);

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _service.GetHistoricalRatesAsync(
                DateOnly.Parse("2024-08-27"), DateOnly.Parse("2024-08-28"), "USD", "EUR", 100)
            );
        }
    }
}
