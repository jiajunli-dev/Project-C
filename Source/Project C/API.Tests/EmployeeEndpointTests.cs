namespace API.Tests;

[TestClass]
public class EmployeeEndpointTests : TestBase
{
    [TestMethod]
    [DataRow("Get", "Employee")]
    [DataRow("Post", "Employee")]
    [DataRow("Put", "Employee")]
    [DataRow("Get", "Employee/1")]
    [DataRow("Delete", "Employee/1")]
    public async Task Endpoints_ReturnUnauthorized(string httpMethod, string url)
    {
        // Arrange
        var client = CreateClient();

        var method = httpMethod switch
        {
            "Get" => HttpMethod.Get,
            "Post" => HttpMethod.Post,
            "Put" => HttpMethod.Put,
            "Delete" => HttpMethod.Delete,
            _ => throw new NotImplementedException()
        };

        // Act
        var response = await client.SendAsync(new HttpRequestMessage(method, url));

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    [DataRow("Get", "Employee", Roles.ADMIN, true)]
    [DataRow("Get", "Employee", Roles.EMPLOYEE, true)]
    [DataRow("Get", "Employee", Roles.CUSTOMER)]
    [DataRow("Get", "Employee")]

    [DataRow("Get", "Employee/1", Roles.ADMIN, true)]
    [DataRow("Get", "Employee/1", Roles.EMPLOYEE, true)]
    [DataRow("Get", "Employee/1", Roles.CUSTOMER, true)]
    [DataRow("Get", "Employee/1")]

    [DataRow("Post", "Employee", Roles.ADMIN, true)]
    [DataRow("Post", "Employee", Roles.EMPLOYEE, true)]
    [DataRow("Post", "Employee", Roles.CUSTOMER, true)]
    [DataRow("Post", "Employee")]

    [DataRow("Put", "Employee", Roles.ADMIN, true)]
    [DataRow("Put", "Employee", Roles.EMPLOYEE, true)]
    [DataRow("Put", "Employee", Roles.CUSTOMER)]
    [DataRow("Put", "Employee")]

    [DataRow("Delete", "Employee/1", Roles.ADMIN, true)]
    [DataRow("Delete", "Employee/1", Roles.EMPLOYEE)]
    [DataRow("Delete", "Employee/1", Roles.CUSTOMER)]
    [DataRow("Delete", "Employee/1")]
    public async Task Endpoints_EnsureAuthorization(string method, string endpoint, string? role = null, bool isAuthorized = false)
    {
        var client = role switch
        {
            Roles.ADMIN => CreateAdminClient(),
            Roles.EMPLOYEE => CreateEmployeeClient(),
            Roles.CUSTOMER => CreateCustomerClient(),
            _ => CreateClient(),
        };
        var httpMethod = method switch
        {
            "Get" => HttpMethod.Get,
            "Post" => HttpMethod.Post,
            "Put" => HttpMethod.Put,
            "Delete" => HttpMethod.Delete,
            _ => throw new ArgumentException("Invalid HTTP method", nameof(method)),
        };

        var result = await client.SendAsync(new HttpRequestMessage(httpMethod, endpoint));

        if (isAuthorized)
            Assert.IsTrue(result.StatusCode is not HttpStatusCode.Forbidden and not HttpStatusCode.Unauthorized);
        else
            Assert.IsTrue(result.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public async Task Get_AllReturnNoContent()
    {
        // Arrange
        var client = CreateAdminClient();
        var repsonse = await client.GetAsync("Employee");
        Assert.IsNotNull(repsonse);
        if (repsonse.StatusCode == HttpStatusCode.OK)
        {
            var employees = await repsonse.Content.ReadFromJsonAsync<List<Employee>>();
            if (employees != null)
                foreach (var employee in employees)
                    await client.DeleteAsync($"Employee/{employee.Id}");
        }

        // Act
        var reposne = await client.GetAsync("Employee");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, reposne.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnOK()
    {
        // Arrange
        var client = CreateAdminClient();
        for (int i = 1; i <= 2; i++)
        {
            await client.PostAsJsonAsync("Employee", new Employee
            {
                PhoneNumber = "12345678",
                DepartmentId = 1,
            });
        }

        // Act
        var response = await client.GetAsync("Employee");
        var model = await response.Content.ReadFromJsonAsync<List<Employee>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(model);
        Assert.IsTrue(model.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnOK()
    {
        // Arrange
        var client = CreateAdminClient();

        var model = new Employee
        {
            PhoneNumber = "12345678",
            DepartmentId = 2,
        };
        var response = await client.PostAsJsonAsync("Employee", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Employee>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.GetAsync($"Employee/{responseModel.Id}");
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Employee>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(responseModel.Id, responseModel2.Id);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnNoContent()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.GetAsync("Employee/'-1'");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Post_CreateReturnsCreated()
    {
        // Arrange
        var client = CreateAdminClient();
        var model = new Employee
        {
            PhoneNumber = "12345678",
            DepartmentId = 3,
        };

        // Act
        var response = await client.PostAsJsonAsync("Employee", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Employee>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(model.PhoneNumber, responseModel.PhoneNumber);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PostAsJsonAsync("Employee", new Employee { });

        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOK()
    {
        // Arrange
        var client = CreateAdminClient();
        var model = new Employee
        {
            PhoneNumber = "12345678",
            DepartmentId = 4,
        };
        var response = await client.PostAsJsonAsync("Employee", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Employee>();
        Assert.IsNotNull(responseModel);

        // Act
        var expected = "87654321";
        responseModel.PhoneNumber = expected;
        var response2 = await client.PutAsJsonAsync("Employee", responseModel);
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Employee>();

        // Assert
        response2.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(expected, responseModel2.PhoneNumber);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PutAsJsonAsync("Employee", new Employee { });
        var response2 = await client.PutAsJsonAsync("Employee", new Employee { Id = "-1" });

        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.ThrowsException<HttpRequestException>(response2.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsNoContent()
    {
        // Arrange
        var client = CreateAdminClient();
        var model = new Employee
        {
            PhoneNumber = "12345678",
            DepartmentId = 5,
        };
        var response = await client.PostAsJsonAsync("Employee", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Employee>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.DeleteAsync($"Employee/{responseModel.Id}");

        // Assert
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.DeleteAsync("Employee/'-1'");
        var response2 = await client.DeleteAsync("Employee/'1000'");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }
}