namespace API.Tests;

[TestClass]
public class EmployeeEndpointTests : TestBase
{
    private const string _endpoint = "Employee";

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
        => await base.Endpoints_EnsureAuthorization(method, endpoint, role, isAuthorized);

    [TestMethod]
    public async Task Get_AllReturnsNoContent()
    {
        // Arrange
        var client = CreateAdminClient();
        await DeleteAllExistingEmployees();

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
            await CreateEmployeeInDb(i);

        // Act
        var result = await client.GetAsync(_endpoint);
        result.EnsureSuccessStatusCode();
        var model = await result.Content.ReadFromJsonAsync<List<Employee>>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(model);
        Assert.IsTrue(model.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsOK()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = await CreateEmployeeInDb(3);

        // Act
        var result = await client.GetAsync($"{_endpoint}/{expectedModel.Id}");
        var resultModel = await result.Content.ReadFromJsonAsync<Employee>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        foreach (var property in typeof(Employee).GetProperties())
            Assert.AreEqual(property.GetValue(expectedModel), property.GetValue(resultModel));
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsNoContent()
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
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Employee>();

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.CreatedBy);
        Assert.AreEqual(expectedModel.Username, resultModel.Username);
        Assert.AreEqual(expectedModel.FirstName, resultModel.FirstName);
        Assert.AreEqual(expectedModel.LastName, resultModel.LastName);
        Assert.AreEqual(expectedModel.Email, resultModel.Email);
        Assert.AreEqual(expectedModel.PhoneNumber, resultModel.PhoneNumber);
        Assert.AreEqual(expectedModel.DepartmentId, resultModel.DepartmentId);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PostAsJsonAsync(_endpoint, new CreateEmployeeDto { });

        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOK()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = await CreateEmployeeInDb(5);
        var expected = "87654321";

        // Act
        expectedModel.PhoneNumber = expected;
        var result = await client.PutAsJsonAsync(_endpoint, expectedModel);
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Employee>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expected, resultModel.PhoneNumber);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PutAsJsonAsync(_endpoint, new object { });
        var response2 = await client.PutAsJsonAsync(_endpoint, new Employee { });
        var response3 = await client.PutAsJsonAsync(_endpoint, new Employee { Id =-1 });

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
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = await CreateEmployeeInDb(6);

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
        var response = await client.DeleteAsync($"{_endpoint}/'-1'");
        var response2 = await client.DeleteAsync($"{_endpoint}/'1000'");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    private static CreateEmployeeDto CreateDto(int i = 1) => new()
    {
        CreatedBy = $"Test{i}",
        Username = $"Test{i}",
        FirstName = $"Test{i}",
        LastName = $"Test{i}",
        Email = $"test{i}@test.nl",
        PhoneNumber = "1234567890",
        DepartmentId = 1
    };

    private async Task<Employee> CreateEmployeeInDb(int i = 1)
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
        var resultModel = await result.Content.ReadFromJsonAsync<Employee>();
        if (resultModel is null)
            Assert.Inconclusive("Failed to create employee");
        return resultModel;
    }

    private async Task DeleteAllExistingEmployees()
    {
        var client = CreateAdminClient();
        var response = await client.GetAsync(_endpoint);
        if (response.StatusCode == HttpStatusCode.NoContent)
            return;

        var models = await response.Content.ReadFromJsonAsync<List<Employee>>();
        if (models is not null && models.Any())
        {
            var tasks = models.Select(model => client.DeleteAsync($"{_endpoint}/{model.Id}"));
            await Task.WhenAll(tasks);
            if (tasks.Any(task => task.Result.StatusCode != HttpStatusCode.NoContent))
                Assert.Inconclusive("Unable to delete all employees");
        }
    }
}