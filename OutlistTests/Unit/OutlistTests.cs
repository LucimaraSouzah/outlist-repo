using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OutlistTests.Unit
{
    public class OutlistTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public OutlistTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        }

        public record OutlistProductDto(Guid ProductId, DateTime StartDate, DateTime EndDate);

        [Fact]
        public async Task AddProduct_ShouldReturnSuccess()
        {
            var dto = new OutlistProductDto(
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7));

            var response = await _client.PostAsJsonAsync("/api/outlist", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            if (response.Content.Headers.ContentLength > 0)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.False(string.IsNullOrEmpty(content));
            }
        }


        [Fact]
        public async Task RemoveProduct_ShouldReturnNoContent()
        {
            var productId = Guid.NewGuid();

            var addDto = new OutlistProductDto(productId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));
            await _client.PostAsJsonAsync("/api/outlist", addDto);

            var response = await _client.DeleteAsync($"/api/outlist/{productId}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateValidity_ShouldReturnNoContent()
        {
            var productId = Guid.NewGuid();

            var addDto = new OutlistProductDto(productId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));
            await _client.PostAsJsonAsync("/api/outlist", addDto);

            var updateDto = new
            {
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            var response = await _client.PutAsJsonAsync($"/api/outlist/{productId}", updateDto);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/outlist?page=1&pageSize=20");
            response.EnsureSuccessStatusCode();

            var list = await response.Content.ReadFromJsonAsync<List<OutlistProductDto>>();
            Assert.NotNull(list);
            Assert.True(list.Count <= 200);
        }

        [Fact]
        public async Task CheckProductBlocked_ShouldReturnBoolean()
        {
            var productId = Guid.NewGuid();

            var responseFalse = await _client.GetAsync($"/api/outlist/check/{productId}");
            responseFalse.EnsureSuccessStatusCode();
            var isBlockedFalse = await responseFalse.Content.ReadFromJsonAsync<bool>();
            Assert.False(isBlockedFalse);

            var addDto = new OutlistProductDto(productId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));
            await _client.PostAsJsonAsync("/api/outlist", addDto);

            var responseTrue = await _client.GetAsync($"/api/outlist/check/{productId}");
            responseTrue.EnsureSuccessStatusCode();
            var isBlockedTrue = await responseTrue.Content.ReadFromJsonAsync<bool>();
            Assert.True(isBlockedTrue);
        }
    }
}
