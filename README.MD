# Author: Korawat Plainsuwan

# Setup
``` bash
git clone https://github.com/6410685066/dev_assessment.git
cd dev_assessment
cd intergold-assessment        
dotnet restore                 
dotnet build                   
dotnet test                    
```

## Part 1: Analyze Legacy Code
### 1. In your own words, what is the primary purpose of this function?
- The primary purpose of the function is to connect to the database and retrieve customer information based on the specified customer ID.

### 2. Identify at least three distinct problems with this implementation. Consider aspects such as security, maintainability, and performance.
1. Security – SQL Injection
The function builds the SQL query using string concatenation, which introduces a critical security vulnerability (SQL injection). An attacker could manipulate the id input to execute arbitrary SQL commands.
2. Maintainability – Hardcoded Table and Column Names
Table and column names are hardcoded directly in the query. If the database schema changes (e.g., table or column renaming), the developer must manually update every occurrence of these names throughout the codebase.
3. Performance – Use of SELECT *
Using SELECT * retrieves all columns, including unnecessary or sensitive data such as passwords or tokens. This not only impacts performance due to excessive data transfer but also increases security risks by exposing sensitive information.

## 3. For each problem identified, briefly propose a specific improvement
1. Avoid string concatenation and instead use parameterized queries (e.g., WHERE id = @id) to prevent SQL injection.
2. Use an ORM (Object-Relational Mapping): Replacing raw SQL with an ORM improves security and maintainability. The ORM handles parameter binding and maps columns to properties automatically, reducing the risk of human error.
3. Select only necessary columns: Fetch only the fields that are required or use DTOs (Data Transfer Objects) to limit the output and enhance both performance and security.


## Part 2: Rewrite and Modernize
### choose C#(.NET9.0 with Entity Framework Core)
```csharp
public async Task<CustomerInfoDto?> GetCustomerInfo2(int id)
{
    var customer = await _dbContext.Customers
        .Where(c => c.Id == id)
        .Select(c => new CustomerInfoDto
        {
            Id = c.Id,
            FirstName = c.FirstName,
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
```


## Part 3: Extend with New Logic
### 1. modern, rewritten version
```csharp
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
            FirstName = c.FirstName,
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
```
### 2. original legacy C# code
```csharp
public DataTable GetCustomerInfo(string id, DateTime? startDate = null, DateTime? endDate = null) 
{ 
    var dt = new DataTable(); 
    using (var conn = new SqlConnection("...")) // Connection string is hardcoded  
    { 
        conn.Open();
        var sql = """
            SELECT * 
            FROM Customers c
            WHERE c.Id = @id
              AND (@startDate IS NULL OR c.CreatedAt >= @startDate)
              AND (@endDate IS NULL OR c.CreatedAt <= @endDate)
            """;

        using (var cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@startDate", (object?)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@endDate", (object?)endDate ?? DBNull.Value);

            using (var adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dt);
            }
        }
    } 
    return dt; 
} 
```


## Part 4 (Optional): System and User Perspective
### 1. What input fields would a user (e.g., a customer service representative) need on the screen to search for customer data?
- Customer ID (required): Used to directly identify and retrieve a specific customer record.
-Start Date and End Date (optional): These fields allow the user to filter by the customer's creation date. They serve as additional constraints to ensure the result is scoped to a particular time period

### 2. After the search is performed, what would the output look like on their screen? (e.g., a table, a profile card, a list of transactions).
- The result should be presented as a profile card showing key customer information, matching the structure defined in the CustomerInfoDto
```csharp
public class CustomerInfoDto
{
    public int Id { get; set; }

    public string FirstNameName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? LastLogin { get; set; }

    public string Status { get; set; } = "A";
}
```
- This profile card may also include action buttons like View, Edit, Delete, or Suspend, depending on the internal dashboard's functionality.

### Summary
- This function is primarily intended to retrieve a single customer by ID. While it supports optional filtering by creation date, it is not designed for general searching or listing. The retrieved customer data can then be used for further CRUD operations or other customer service actions within the system.

## Tools & Resources Used
- .NET 9.0
- Entity Framework Core
- Visual Studio Code
- Git
- GitHub Copilot
- ChatGPT
