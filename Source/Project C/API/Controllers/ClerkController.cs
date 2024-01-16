using System.Text;

using Data.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ClerkController : ControllerBase
{
    private readonly ILogger<ClerkController> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICustomerRepository _customerRepository;

    public ClerkController(ILogger<ClerkController> logger,
                           IEmployeeRepository employeeRepository,
                           ICustomerRepository customerRepository)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
        _customerRepository = customerRepository;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> UserCreated()
    {
        _logger.LogInformation("User created account in Clerk");

        using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
        var result = await reader.ReadToEndAsync();

        var response = JsonConvert.DeserializeObject<UserCreatedResponse>(result);

        _logger.LogInformation($"User created account in Clerk: {response.data.id}");

        try
        {
            var email = response.data.email_addresses.FirstOrDefault(c => c.id == response.data.primary_email_address_id);
            if (email is null)
                return Ok();

            var role = response.data.public_metadata.FirstOrDefault(c => c.Key == "role").Value;
            if (role is null)
                return Ok();

            switch (role)
            {
                case "admin":
                case "employee":
                    var user = await _employeeRepository.GetByEmail(email.email_address);
                    if (user is null)
                        return Ok();

                    user.ClerkId = response.data.id;
                    await _employeeRepository.Update(user);
                    break;
                case "customer":
                    var customer = await _customerRepository.GetByEmail(email.email_address);
                    if (customer is null)
                        return Ok();

                    customer.ClerkId = response.data.id;
                    await _customerRepository.Update(customer);
                    break;
                default:
                    break;
            }


            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing a new user account in clerk");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}

public class EmailAddress
{
    public string email_address { get; set; }
    public string id { get; set; }
}

public class Data
{
    public long created_at { get; set; }
    public long updated_at { get; set; }
    public List<EmailAddress> email_addresses { get; set; }
    public string first_name { get; set; }
    public string id { get; set; }
    public string last_name { get; set; }
    public string primary_email_address_id { get; set; }
    public Dictionary<string, string> public_metadata { get; set; }
    public object username { get; set; }
}

public class UserCreatedResponse
{
    public Data data { get; set; }
}
