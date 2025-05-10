using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using EShopDomain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EShopService.IntegrationTests.Controllers
{
    public class ProductControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public ProductControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // pobranie dotychczasowej konfiguracji bazy danych
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                        //// usunięcie dotychczasowej konfiguracji bazy danych
                        services.Remove(dbContextOptions);

                        // Stworzenie nowej bazy danych
                        services
                            .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));

                    });
                });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsAllProducts()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                // Pobranie kontekstu bazy danych
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                dbContext.Products.RemoveRange(dbContext.Products);

                // Stworzenie obiektu
                dbContext.Products.AddRange(
                    new Product { Name = "Product1" },
                    new Product { Name = "Product2" }
                );
                // Zapisanie obiektu
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/product");

            // Assert
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.Equal(2, products?.Count);
        }

    }
}
