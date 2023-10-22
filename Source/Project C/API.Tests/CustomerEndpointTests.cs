namespace API.Tests;

[TestClass]
public class CustomerEndpointTests : TestBase
{
    private const string _endpoint = "Customer";

    [TestMethod]
    [DataRow("Get", _endpoint, Roles.ADMIN, true)]
    [DataRow("Get", _endpoint, Roles.EMPLOYEE, true)]
    [DataRow("Get", _endpoint, Roles.CUSTOMER)]
    [DataRow("Get", _endpoint)]

    [DataRow("Get", $"{_endpoint}/1", Roles.ADMIN, true)]
    [DataRow("Get", $"{_endpoint}/1", Roles.EMPLOYEE, true)]
    [DataRow("Get", $"{_endpoint}/1", Roles.CUSTOMER, true)]
    [DataRow("Get", $"{_endpoint}/1")]

    [DataRow("Post", _endpoint, Roles.ADMIN, true)]
    [DataRow("Post", _endpoint, Roles.EMPLOYEE, true)]
    [DataRow("Post", _endpoint, Roles.CUSTOMER, true)]
    [DataRow("Post", _endpoint)]

    [DataRow("Put", _endpoint, Roles.ADMIN, true)]
    [DataRow("Put", _endpoint, Roles.EMPLOYEE, true)]
    [DataRow("Put", _endpoint, Roles.CUSTOMER)]
    [DataRow("Put", _endpoint)]

    [DataRow("Delete", $"{_endpoint}/1", Roles.ADMIN, true)]
    [DataRow("Delete", $"{_endpoint}/1", Roles.EMPLOYEE)]
    [DataRow("Delete", $"{_endpoint}/1", Roles.CUSTOMER)]
    [DataRow("Delete", $"{_endpoint}/1")]
    public override async Task Endpoints_EnsureAuthorization(string method,
                                                string endpoint,
                                                string? role = null,
                                                bool isAuthorized = false)
        => await base.Endpoints_EnsureAuthorization(method,
                                                    endpoint,
                                                    role,
                                                    isAuthorized);

    [TestMethod]
    public async Task Get_GetAllReturnsNoContent()
    {
        // Arrange
        var client = CreateAdminClient();
        await DeleteAllExistingCustomers(client);

        // Act
        var result = await client.GetAsync(_endpoint);

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnsOK()
    {
        // Arrange
        var client = CreateAdminClient();
        for (int i = 1; i <= 2; i++)
            await CreateCustomerInDb(i);

        // Act
        var result = await client.GetAsync(_endpoint);
        result.EnsureSuccessStatusCode();
        var models = await result.Content.ReadFromJsonAsync<List<Customer>>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(models);
        Assert.IsTrue(models.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = await CreateCustomerInDb(3);

        // Act
        var result = await client.GetAsync($"{_endpoint}/{expectedModel.Id}");
        var resultModel = await result.Content.ReadFromJsonAsync<Customer>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        foreach (var property in typeof(Customer).GetProperties())
            Assert.AreEqual(property.GetValue(expectedModel), property.GetValue(resultModel));
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.GetAsync($"{_endpoint}/-1");
        var response2 = await client.GetAsync($"{_endpoint}/0");
        var response3 = await client.GetAsync($"{_endpoint}/99999");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response3.StatusCode);
    }

    [TestMethod]
    public async Task Post_CreateReturnsCreated()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = CreateDto(4);

        // Act
        var result = await client.PostAsJsonAsync(_endpoint, expectedModel);
        var resultModel = await result.Content.ReadFromJsonAsync<Customer>();
        result.EnsureSuccessStatusCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expectedModel.Id, resultModel.Id);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.CreatedBy);
        Assert.AreEqual(expectedModel.Username, resultModel.Username);
        Assert.AreEqual(expectedModel.FirstName, resultModel.FirstName);
        Assert.AreEqual(expectedModel.LastName, resultModel.LastName);
        Assert.AreEqual(expectedModel.Email, resultModel.Email);
        Assert.AreEqual(expectedModel.PhoneNumber, resultModel.PhoneNumber);
        Assert.AreEqual(expectedModel.CompanyName, resultModel.CompanyName);
        Assert.AreEqual(expectedModel.CompanyPhoneNumber, resultModel.CompanyPhoneNumber);
        Assert.AreEqual(expectedModel.DepartmentName, resultModel.DepartmentName);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var result = await client.PostAsJsonAsync("Customer", new CreateCustomerDto { });

        // Assert
        Assert.ThrowsException<HttpRequestException>(result.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = await CreateCustomerInDb(5);
        var expected = "This is a test";

        // Act
        expectedModel.CompanyName = expected;
        var result = await client.PutAsJsonAsync(_endpoint, expectedModel);
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Customer>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expected, resultModel.CompanyName);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PutAsJsonAsync(_endpoint, new object { });
        var response2 = await client.PutAsJsonAsync(_endpoint, new Customer { });
        var response3 = await client.PutAsJsonAsync(_endpoint, new Customer { Id = "-1" });

        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.ThrowsException<HttpRequestException>(response2.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
        Assert.ThrowsException<HttpRequestException>(response3.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response3.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsNoContent()
    {
        var client = CreateAdminClient();
        var expectedModel = await CreateCustomerInDb(6);

        // Act
        var result = await client.DeleteAsync($"{_endpoint}/{expectedModel.Id}");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.DeleteAsync($"{_endpoint}/-1");
        var response2 = await client.DeleteAsync($"{_endpoint}/1000");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    private static CreateCustomerDto CreateDto(int i = 1) => new()
    {
        Id = $"Test_{i}",
        CreatedBy = $"Test{i}",
        Username = $"Test{i}",
        FirstName = $"Test{i}",
        LastName = $"Test{i}",
        Email = $"test{i}@test.nl",
        PhoneNumber = "1234567890",
        CompanyName = $"Test{i} Company",
        CompanyPhoneNumber = "1234567890",
        DepartmentName = $"Test{i} Department"
    };

    private async Task<Customer> CreateCustomerInDb(int i = 1)
    {
        var model = CreateDto(i);
        var client = CreateAdminClient();
        var result = await client.PostAsJsonAsync(_endpoint, model);
        try
        {
            result.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            Assert.Inconclusive($"Unable to create model for test: {e.StatusCode}");
        }
        var resultModel = await result.Content.ReadFromJsonAsync<Customer>();
        if (resultModel is null)
            Assert.Inconclusive("Failed to create customer");
        return resultModel;
    }

    private static async Task DeleteAllExistingCustomers(HttpClient client)
    {
        var response = await client.GetAsync(_endpoint);
        if (response.StatusCode == HttpStatusCode.NoContent)
            return;

        var models = await response.Content.ReadFromJsonAsync<List<Customer>>();
        if (models is not null && models.Any())
        {
            var tasks = models.Select(customer => client.DeleteAsync($"{_endpoint}/{customer.Id}"));
            await Task.WhenAll(tasks);
            if (tasks.Any(task => task.Result.StatusCode != HttpStatusCode.NoContent))
                Assert.Inconclusive("Unable to delete all customers");
        }
    }
}
