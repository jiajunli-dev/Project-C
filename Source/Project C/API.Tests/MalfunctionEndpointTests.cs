namespace API.Tests;

[TestClass]
public class MalfunctionEndpointTests : TestBase
{
    private const string _endpoint = "Malfunction";

    private static CreateMalfunctionDto CreateDto(string description = "test",
                                                  string solution = "test",
                                                  Priority priority = Priority.Critical,
                                                  Status status = Status.Open,
                                                  int ticketId = 0) => new()
                                                  {
                                                      CreatedBy = "user_2WnUOUCmtnMwnZQpwvBLnBnC6tc",
                                                      TicketId = ticketId,
                                                      Description = description,
                                                      Solution = solution,
                                                      Priority = priority,
                                                      Status = status
                                                  };

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
    public async Task Endpoints_EnsureAuthorization(string method,
                                                    string endpoint,
                                                    string? role = null,
                                                    bool isAuthorized = false)
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
    public async Task Get_GetAllReturnsNoContent()
    {
        var client = CreateAdminClient();
        var response = await client.GetAsync(_endpoint);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var malfunctions = await response.Content.ReadFromJsonAsync<List<Malfunction>>();
            if (malfunctions is not null)
                foreach (var malfunction in malfunctions)
                    await client.DeleteAsync($"{_endpoint}/{malfunction.Id}");
        }

        var result = await client.GetAsync(_endpoint);

        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnsOK()
    {
        var client = CreateAdminClient();
        for (int i = 1; i <= 2; i++)
            await client.PostAsJsonAsync(_endpoint, CreateDto($"Test{i}", $"Test{i}"));

        var response = await client.GetAsync(_endpoint);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<List<Malfunction>>();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.IsTrue(responseModel.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsOk()
    {
        var client = CreateAdminClient();
        var model = CreateDto();
        var response = await client.PostAsJsonAsync(_endpoint, model);
        var expectedModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(expectedModel);

        var result = await client.GetAsync($"{_endpoint}/{expectedModel.Id}");
        var resultModel = await result.Content.ReadFromJsonAsync<Malfunction>();

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expectedModel.Id, resultModel.Id);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.CreatedBy);
        Assert.AreEqual(expectedModel.CreatedAt, resultModel.CreatedAt);
        Assert.AreEqual(expectedModel.UpdatedAt, resultModel.UpdatedAt);
        Assert.AreEqual(expectedModel.UpdatedBy, resultModel.UpdatedBy);
        Assert.AreEqual(expectedModel.Priority, resultModel.Priority);
        Assert.AreEqual(expectedModel.Status, resultModel.Status);
        Assert.AreEqual(expectedModel.Description, resultModel.Description);
        Assert.AreEqual(expectedModel.Solution, resultModel.Solution);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.GetAsync($"{_endpoint}/-1");
        var response2 = await client.GetAsync($"{_endpoint}/0");
        var response3 = await client.GetAsync($"{_endpoint}/99999");

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response3.StatusCode);
    }

    [TestMethod]
    public async Task Post_CreateReturnsCreated()
    {
        var client = CreateAdminClient();
        var model = CreateDto();

        var response = await client.PostAsJsonAsync(_endpoint, model);
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        response.EnsureSuccessStatusCode();

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(model.CreatedBy, responseModel.CreatedBy);
        Assert.AreEqual(model.CreatedBy, responseModel.UpdatedBy);
        Assert.AreEqual(model.Priority, responseModel.Priority);
        Assert.AreEqual(model.Status, responseModel.Status);
        Assert.AreEqual(model.Description, responseModel.Description);
        Assert.AreEqual(model.Solution, responseModel.Solution);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.PostAsJsonAsync(_endpoint, new object { });

        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOk()
    {
        var client = CreateAdminClient();
        var model = CreateDto();
        var response = await client.PostAsJsonAsync(_endpoint, model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(responseModel);
        var expected = "This is a changed value";

        responseModel.Description = expected;
        var response2 = await client.PutAsJsonAsync(_endpoint, responseModel);
        response2.EnsureSuccessStatusCode();
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Malfunction>();

        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(expected, responseModel2.Description);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.PutAsJsonAsync(_endpoint, new object { });
        var response2 = await client.PutAsJsonAsync(_endpoint, new Malfunction { Id = -1 });
        var response3 = await client.PutAsJsonAsync(_endpoint, new Malfunction { Id = 0 });

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
        var model = CreateDto();
        var response = await client.PostAsJsonAsync(_endpoint, model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(responseModel);

        var result = await client.DeleteAsync($"{_endpoint}/{responseModel.Id}");

        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.DeleteAsync($"{_endpoint}/-1");
        var response2 = await client.DeleteAsync($"{_endpoint}/2222");

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }
}
