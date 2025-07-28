using InterGoldAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace InterGoldAssessment.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _dbContext;

        public CustomerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Anser for Part 2
        public async Task<CustomerInfoDto?> GetCustomerInfo2(int id)
        {
            var customer = await _dbContext.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomerInfoDto
                {
                    Id = c.Id,
                    FirstNameName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CreatedAt = c.CreatedAt,
                    PhoneNumber = c.PhoneNumber,
                    LastLogin = c.LastLogin,
                    Status = c.Status
                })
                .FirstOrDefaultAsync();

            return customer;
        }

        // Answer for Part 3.1 
        public async Task<CustomerInfoDto?> GetCustomerInfo3(int id, DateTime? startDate = null, DateTime? endDate = null)
        {
            var customer = await _dbContext.Customers
                .Where(c => c.Id == id &&
                            (!startDate.HasValue || c.CreatedAt >= startDate.Value) &&
                            (!endDate.HasValue || c.CreatedAt <= endDate.Value)
                            )
                .Select(c => new CustomerInfoDto
                {
                    Id = c.Id,
                    FirstNameName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CreatedAt = c.CreatedAt,
                    PhoneNumber = c.PhoneNumber,
                    LastLogin = c.LastLogin,
                    Status = c.Status
                })
                .FirstOrDefaultAsync();

            return customer;
        }
    }
}
