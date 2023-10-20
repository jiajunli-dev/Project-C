namespace API.Tests;

[TestClass]
public class PhotoEndpointTests : TestBase
{
    private const string _endpoint = "Photo";
    private readonly CreateTicketDto _createTicketDto = new()
    {
        CreatedBy = "123",
        AdditionalNotes = "Test",
        TriedSolutions = "Test",
        Description = "Test",
        Priority = Priority.Critical
    };

    [TestMethod]
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
    [DataRow("Put", _endpoint, Roles.CUSTOMER, true)]
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
    public async Task Get_GetByIdReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var ticketResult = await client.PostAsJsonAsync("Ticket", _createTicketDto);
        var ticket = await ticketResult.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(ticket);

        var response = await client.PostAsJsonAsync(_endpoint, CreatePhotoDto(ticket.Id));
        var responseModel = await response.Content.ReadFromJsonAsync<Photo>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.GetAsync($"{_endpoint}/{responseModel.Id}");
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Photo>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(responseModel.Id, responseModel2.Id);
        Assert.AreEqual(ticket.Id, responseModel2.TicketId);
        Assert.AreEqual(responseModel.CreatedBy, responseModel2.CreatedBy);
        Assert.AreEqual(responseModel.CreatedAt, responseModel2.CreatedAt);
        Assert.AreEqual(responseModel.UpdatedAt, responseModel2.UpdatedAt);
        Assert.AreEqual(responseModel.UpdatedBy, responseModel2.UpdatedBy);
        Assert.AreEqual(responseModel.Name, responseModel2.Name);
        CollectionAssert.AreEqual(responseModel.Data, responseModel2.Data);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.GetAsync($"{_endpoint}/-1");
        var response2 = await client.GetAsync($"{_endpoint}/0");
        var response3 = await client.GetAsync($"{_endpoint}/3834");

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
        var response = await client.PostAsJsonAsync("Ticket", _createTicketDto);
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(ticket);
        var photo = CreatePhotoDto(ticket.Id);

        // Act
        var response2 = await client.PostAsJsonAsync(_endpoint, photo);
        var responseModel = await response2.Content.ReadFromJsonAsync<Photo>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(ticket.Id, responseModel.TicketId);
        Assert.AreEqual(photo.CreatedBy, responseModel.CreatedBy);
        Assert.AreEqual(photo.CreatedBy, responseModel.UpdatedBy);
        Assert.AreEqual(photo.Name, responseModel.Name);
        CollectionAssert.AreEqual(Convert.FromBase64String(photo.Data), responseModel.Data);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PostAsJsonAsync(_endpoint, new object { });
        var response2 = await client.PostAsJsonAsync(_endpoint, new Photo { TicketId = -1 });
        var response3 = await client.PostAsJsonAsync(_endpoint, new Photo { TicketId = 384 });
        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.ThrowsException<HttpRequestException>(response2.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
        Assert.ThrowsException<HttpRequestException>(response3.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response3.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var expected = "This is a changed value";
        var ticketResponse = await client.PostAsJsonAsync("Ticket", _createTicketDto);
        ticketResponse.EnsureSuccessStatusCode();
        var ticket = await ticketResponse.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(ticket);
        var photoResponse = await client.PostAsJsonAsync(_endpoint, CreatePhotoDto(ticket.Id));
        var photo = await photoResponse.Content.ReadFromJsonAsync<Photo>();
        Assert.IsNotNull(photo);

        // Act
        photo.Name = expected;
        var result = await client.PutAsJsonAsync(_endpoint, new PhotoDto(photo));
        result.EnsureSuccessStatusCode();
        var resultModel = await result.Content.ReadFromJsonAsync<Photo>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultModel);
        Assert.AreEqual(expected, resultModel.Name);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PutAsJsonAsync(_endpoint, new object { });
        var response2 = await client.PutAsJsonAsync(_endpoint, new PhotoDto { TicketId = -1 });
        var response3 = await client.PutAsJsonAsync(_endpoint, new PhotoDto { TicketId = 2734 });

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
        var response = await client.PostAsJsonAsync("Ticket", _createTicketDto);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

        var response2 = await client.PostAsJsonAsync(_endpoint, CreatePhotoDto(responseModel.Id));
        var responseModel2 = await response2.Content.ReadFromJsonAsync<PhotoDto>();
        Assert.IsNotNull(responseModel2);

        // Act
        var response3 = await client.DeleteAsync($"{_endpoint}/{responseModel2.Id}");

        // Assert
        Assert.IsNotNull(response3);
        Assert.AreEqual(HttpStatusCode.NoContent, response3.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.DeleteAsync($"{_endpoint}/-1");
        var response2 = await client.DeleteAsync($"{_endpoint}/2983");

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    private static CreatePhotoDto CreatePhotoDto(int ticketId) => new()
    {
        CreatedBy = "123",
        TicketId = ticketId,
        Name = "Test.png",
        Data = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
    };
}