using Microsoft.EntityFrameworkCore;
using InterGoldAssessment.Entity;
using InterGoldAssessment.Services;

namespace InterGoldAssessment.Tests.Tests.Q2
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task GetCustomerInfo_ReturnsCustomerInfoDto_WhenCustomerExists()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var created_at = DateTime.UtcNow;
            using (var context = new AppDbContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    FirstName = "Somsak",
                    LastName = "Jaidee",
                    Email = "somsak@example.com",
                    CreatedAt = created_at,
                    PasswordHash = "hashed_password",
                    PasswordSalt = "salt",
                    LastLogin = DateTime.UtcNow,
                    PhoneNumber = "1234567890"
                });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new CustomerService(context);
                var result = await service.GetCustomerInfo2(1);

                Assert.NotNull(result);
                Assert.Equal(1, result!.Id);
                Assert.Equal("Somsak", result.FirstNameName);
                Assert.Equal("Jaidee", result.LastName);
                Assert.Equal("somsak@example.com", result.Email);
                Assert.Equal(created_at, result.CreatedAt);
                Assert.NotNull(result.LastLogin);
                Assert.Equal("1234567890", result.PhoneNumber);
                Assert.Equal("A", result.Status);
            }
        }
        
        [Fact]
        public static async Task GetCustomerInfo_ReturnsNull_WhenCustomerDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var service = new CustomerService(context);
                var result = await service.GetCustomerInfo2(999);

                Assert.Null(result);
            }
        }
    }
}
