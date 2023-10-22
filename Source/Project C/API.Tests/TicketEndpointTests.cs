namespace API.Tests;

[TestClass]
public class TicketEndpointTests : TestBase
{
    private const string _endpoint = "Ticket";

    [TestMethod]
    [DataRow("Get", _endpoint, Roles.ADMIN, true)]
    [DataRow("Get", _endpoint, Roles.EMPLOYEE, true)]
    [DataRow("Get", _endpoint, Roles.CUSTOMER)]
    [DataRow("Get", _endpoint)]

    [DataRow("Get", $"{_endpoint}/1", Roles.ADMIN, true)]
    [DataRow("Get", $"{_endpoint}/1", Roles.EMPLOYEE, true)]
    [DataRow("Get", $"{_endpoint}/1", Roles.CUSTOMER, true)]
    [DataRow("Get", $"{_endpoint}/1")]

    [DataRow("Get", $"{_endpoint}/1/photos", Roles.ADMIN, true)]
    [DataRow("Get", $"{_endpoint}/1/photos", Roles.EMPLOYEE, true)]
    [DataRow("Get", $"{_endpoint}/1/photos", Roles.CUSTOMER, true)]
    [DataRow("Get", $"{_endpoint}/1/photos")]

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
        await DeleteAllExistingTickets();

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
            await client.PostAsJsonAsync(_endpoint, CreateDto(i));

        // Act
        var result = await client.GetAsync(_endpoint);
        result.EnsureSuccessStatusCode();
        var models = await result.Content.ReadFromJsonAsync<List<Ticket>>();

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
        var expectedModel = await CreateTicketInDb(3);

        // Act
        var result = await client.GetAsync($"{_endpoint}/{expectedModel.Id}");
        var resultModel = await result.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        foreach (var property in typeof(Ticket).GetProperties())
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
    public async Task Get_GetAllByTicketIdReturnsNoContent()
    {
        // Arrange
        var client = CreateAdminClient();
        var responseModel = await CreateTicketInDb(4);

        // Act
        var response2 = await client.GetAsync($"{_endpoint}/{responseModel.Id}/photos");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllByTicketIdReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var responseModel = await CreateTicketInDb(5);
        int count = 2;
        for (int i = 1; i <= count; i++)
            await CreatePhotoInDb(responseModel.Id, i);

        // Act
        var result = await client.GetAsync($"{_endpoint}/{responseModel.Id}/photos");
        var photos = await result.Content.ReadFromJsonAsync<List<Photo>>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(photos);
        Assert.IsTrue(photos.Any());
        Assert.IsTrue(photos.TrueForAll(p => p.TicketId == responseModel.Id));
        Assert.AreEqual(count, photos.Count);
    }

    [TestMethod]
    public async Task Post_CreateReturnsCreated()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = CreateDto(6);

        // Act
        var result = await client.PostAsJsonAsync(_endpoint, expectedModel);
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.CreatedBy);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.UpdatedBy);
        Assert.AreEqual(expectedModel.Description, resultModel.Description);
        Assert.AreEqual(expectedModel.TriedSolutions, resultModel.TriedSolutions);
        Assert.AreEqual(expectedModel.AdditionalNotes, resultModel.AdditionalNotes);
        Assert.AreEqual(expectedModel.Priority, resultModel.Priority);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PostAsJsonAsync(_endpoint, new object { });

        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedModel = await CreateTicketInDb(7);
        var expected = "This is a changed value";

        // Act
        expectedModel.AdditionalNotes = expected;
        var result = await client.PutAsJsonAsync(_endpoint, expectedModel);
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expected, resultModel.AdditionalNotes);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PutAsJsonAsync(_endpoint, new object { });
        var response2 = await client.PutAsJsonAsync(_endpoint, new Ticket { });
        var response3 = await client.PutAsJsonAsync(_endpoint, new Ticket { Id = -1 });

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
        var responseModel = await CreateTicketInDb(8);

        // Act
        var result = await client.DeleteAsync($"{_endpoint}/{responseModel.Id}");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);

        // TODO: Ensure related photos are deleted as well
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.DeleteAsync($"{_endpoint}/ -1");
        var response2 = await client.DeleteAsync($"{_endpoint}/ 2983");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    private static CreateTicketDto CreateDto(int i = 1) => new()
    {
        CreatedBy = $"Test_{i}",
        AdditionalNotes = $"Test_{i}",
        TriedSolutions = $"Test_{i}",
        Description = $"Test_{i}",
        Priority = Priority.Critical
    };

    private async Task<Ticket> CreateTicketInDb(int i = 1)
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
        var resultModel = await result.Content.ReadFromJsonAsync<Ticket>();
        if (resultModel is null)
            Assert.Inconclusive("Failed to create ticket");

        return resultModel;
    }

    private async Task CreatePhotoInDb(int ticketId, int i)
    {
        var client = CreateAdminClient();
        var photoResult = await client.PostAsJsonAsync("Photo", new CreatePhotoDto
        {
            CreatedBy = $"Test_{i}",
            TicketId = ticketId,
            Name = $"Test{i}.png",
            Data = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
        });
        if (photoResult.StatusCode != HttpStatusCode.Created)
            Assert.Inconclusive($"Unable to create photo for test: {photoResult.StatusCode}");
    }

    private async Task DeleteAllExistingTickets()
    {
        var client = CreateAdminClient();
        var response = await client.GetAsync(_endpoint);
        if (response.StatusCode == HttpStatusCode.NoContent)
            return;

        var models = await response.Content.ReadFromJsonAsync<List<Ticket>>();
        if (models is not null && models.Any())
        {
            var tasks = models.Select(ticket => client.DeleteAsync($"{_endpoint}/{ticket.Id}"));
            await Task.WhenAll(tasks);
            if (tasks.Any(task => task.Result.StatusCode is not HttpStatusCode.NoContent and not HttpStatusCode.BadRequest))
                Assert.Inconclusive("Unable to delete all tickets");
        }
    }
}
