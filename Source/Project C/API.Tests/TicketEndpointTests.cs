namespace API.Tests;

[TestClass]
public class TicketEndpointTests : TestBase
{
    private const string _endpoint = "Ticket";
    private readonly CreateTicketDto _createDto = new()
    {
        CreatedBy = "123",
        AdditionalNotes = "Test",
        TriedSolutions = "Test",
        Description = "Test",
        Priority = Priority.Critical
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
        // Arrange
        var client = CreateAdminClient();
        var response = await client.GetAsync(_endpoint);
        Assert.IsNotNull(response);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
            if (tickets is not null)
                foreach (var ticket in tickets)
                    await client.DeleteAsync($"{_endpoint}/{ticket.Id}");
        }

        // Act
        var response2 = await client.GetAsync(_endpoint);

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnsOK()
    {
        // Arrange
        var client = CreateAdminClient();
        for (int i = 1; i <= 2; i++)
            await client.PostAsJsonAsync(_endpoint, _createDto);

        // Act
        var response = await client.GetAsync(_endpoint);
        var responseModel = await response.Content.ReadFromJsonAsync<List<Ticket>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.IsTrue(responseModel.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var response = await client.PostAsJsonAsync(_endpoint, _createDto);
        var expectedModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(expectedModel);

        // Act
        var result = await client.GetAsync($"{_endpoint}/{expectedModel.Id}");
        var resultModel = await result.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expectedModel.Id, resultModel.Id);
        Assert.AreEqual(expectedModel.CreatedBy, resultModel.CreatedBy);
        Assert.AreEqual(expectedModel.CreatedAt, resultModel.CreatedAt);
        Assert.AreEqual(expectedModel.UpdatedAt, resultModel.UpdatedAt);
        Assert.AreEqual(expectedModel.UpdatedBy, resultModel.UpdatedBy);
        Assert.AreEqual(expectedModel.Description, resultModel.Description);
        Assert.AreEqual(expectedModel.TriedSolutions, resultModel.TriedSolutions);
        Assert.AreEqual(expectedModel.AdditionalNotes, resultModel.AdditionalNotes);
        Assert.AreEqual(expectedModel.Priority, resultModel.Priority);
        Assert.AreEqual(expectedModel.Status, resultModel.Status);
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
        var response = await client.PostAsJsonAsync(_endpoint, _createDto);
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

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
        var response = await client.PostAsJsonAsync(_endpoint, _createDto);
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);
        int count = 2;
        for (int i = 1; i <= count; i++)
        {
            var photoResult = await client.PostAsJsonAsync("Photo", new CreatePhotoDto
            {
                CreatedBy = "123",
                TicketId = responseModel.Id,
                Name = $"Test{i}.png",
                Data = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
            });
            Assert.AreEqual(HttpStatusCode.Created, photoResult.StatusCode);
        }

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
        var expectedModel = _createDto;

        // Act
        var response = await client.PostAsJsonAsync(_endpoint, expectedModel);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(expectedModel.CreatedBy, responseModel.CreatedBy);
        Assert.AreEqual(expectedModel.CreatedBy, responseModel.UpdatedBy);
        Assert.AreEqual(expectedModel.Description, responseModel.Description);
        Assert.AreEqual(expectedModel.TriedSolutions, responseModel.TriedSolutions);
        Assert.AreEqual(expectedModel.AdditionalNotes, responseModel.AdditionalNotes);
        Assert.AreEqual(expectedModel.Priority, responseModel.Priority);
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
        var expected = "This is a changed value";
        var response = await client.PostAsJsonAsync(_endpoint, _createDto);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

        // Act
        responseModel.AdditionalNotes = expected;
        var response2 = await client.PutAsJsonAsync(_endpoint, responseModel);
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        response2.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(expected, responseModel2.AdditionalNotes);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PutAsJsonAsync(_endpoint, new object { });
        var response2 = await client.PutAsJsonAsync(_endpoint, new Ticket { Id = -1 });

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
        var response = await client.PostAsJsonAsync(_endpoint, _createDto);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.DeleteAsync($"{_endpoint}/{responseModel.Id}");

        // Assert
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);

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
}
