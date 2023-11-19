namespace API.Tests;

[TestClass]
public class MalfunctionEndpointTests : TestBase
{
    private const string _endpoint = "Malfunction";

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
        var client = CreateAdminClient();
        await DeleteAllExistingMalfunctions();

        var result = await client.GetAsync(_endpoint);

        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnsOK()
    {
        var client = CreateAdminClient();
        var expectedTicket = await CreateTicketInDb(1);
        for (int i = 1; i <= 2; i++)
            await CreateMalfunctionInDb(expectedTicket.Id, i);

        var result = await client.GetAsync(_endpoint);
        result.EnsureSuccessStatusCode();
        var models = await result.Content.ReadFromJsonAsync<List<Malfunction>>();

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(models);
        Assert.IsTrue(models.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsOk()
    {
        var client = CreateAdminClient();
        var expectedTicket = await CreateTicketInDb(3);
        var expectedModel = await CreateMalfunctionInDb(expectedTicket.Id, 3);

        var result = await client.GetAsync($"{_endpoint}/{expectedModel.Id}");
        var resultModel = await result.Content.ReadFromJsonAsync<Malfunction>();

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        foreach (var property in typeof(Malfunction).GetProperties())
            Assert.AreEqual(property.GetValue(expectedModel), property.GetValue(resultModel));
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
        var expectedTicket = await CreateTicketInDb(4);
        var expectedModel = CreateMalfunctionDto(expectedTicket.Id, 4);

        var result = await client.PostAsJsonAsync(_endpoint, expectedModel);
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Malfunction>();

        Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.CreatedBy);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.UpdatedBy);
        Assert.AreEqual(expectedModel.Priority, resultModel.Priority);
        Assert.AreEqual(expectedModel.Status, resultModel.Status);
        Assert.AreEqual(expectedModel.Description, resultModel.Description);
        Assert.AreEqual(expectedModel.Solution, resultModel.Solution);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var result = await client.PostAsJsonAsync(_endpoint, new object { });

        Assert.ThrowsException<HttpRequestException>(result.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOk()
    {
        var client = CreateAdminClient();
        var expectedTicket = await CreateTicketInDb(5);
        var expectedModel = await CreateMalfunctionInDb(expectedTicket.Id, 5);
        var expected = "This is a changed value";

        expectedModel.Description = expected;
        var result = await client.PutAsJsonAsync(_endpoint, expectedModel);
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Malfunction>();

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expected, resultModel.Description);
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
        var expectedTicket = await CreateTicketInDb(6);
        var expectedModel = await CreateMalfunctionInDb(expectedTicket.Id, 6);

        var result = await client.DeleteAsync($"{_endpoint}/{expectedModel.Id}");

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

    private static CreateMalfunctionDto CreateMalfunctionDto(int ticketId, int i) => new()
    {
        CreatedBy = $"Test_{i}",
        TicketId = ticketId,
        Description = $"Test_{i}",
        Solution = $"Test_{i}",
        Priority = Priority.Critical,
        Status = Status.Open
    };

    private static CreateTicketDto CreateTicketDto(int i = 1) => new()
    {
        CreatedBy = $"Test_{i}",
        AdditionalNotes = $"Test_{i}",
        TriedSolutions = $"Test_{i}",
        Description = $"Test_{i}",
        Priority = Priority.Critical
    };

    private async Task<Ticket> CreateTicketInDb(int i = 1)
    {
        var model = CreateTicketDto(i);
        var client = CreateAdminClient();
        var result = await client.PostAsJsonAsync("Ticket", model);
        try
        {
            result.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            Assert.Inconclusive($"Unable to create model for test: {e.StatusCode}");
        }
        var resultModel = await result.Content.ReadFromJsonAsync<Ticket>();
        if (resultModel is null)
            Assert.Inconclusive("Failed to create ticket");

        return resultModel;
    }

    private async Task<Malfunction> CreateMalfunctionInDb(int ticketId, int i = 1)
    {
        var model = CreateMalfunctionDto(ticketId, i);
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
        var resultModel = await result.Content.ReadFromJsonAsync<Malfunction>();
        if (resultModel is null)
            Assert.Inconclusive("Failed to create malfunction");

        return resultModel;
    }

    private async Task DeleteAllExistingMalfunctions()
    {
        var client = CreateAdminClient();
        var response = await client.GetAsync(_endpoint);
        if (response.StatusCode == HttpStatusCode.NoContent)
            return;

        var models = await response.Content.ReadFromJsonAsync<List<Malfunction>>();
        if (models is not null && models.Any())
        {
            var tasks = models.Select(model => client.DeleteAsync($"{_endpoint}/{model.Id}"));
            await Task.WhenAll(tasks);
            if (tasks.Any(task => task.Result.StatusCode != HttpStatusCode.NoContent))
                Assert.Inconclusive("Unable to delete all malfunctions");
        }
    }
}
