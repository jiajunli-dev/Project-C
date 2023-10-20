namespace API.Tests;

[TestClass]
public class PhotoEndpointTests : TestBase
{
    private readonly CreateTicketDto _createTicketDto = new()
    {
        CreatedBy = "123",
        AdditionalNotes = "Test",
        TriedSolutions = "Test",
        Description = "Test",
        Priority = Priority.Critical
    };

    [TestMethod]
    [DataRow("Get", "Photo/1")]
    [DataRow("Post", "Photo")]
    [DataRow("Put", "Photo")]
    [DataRow("Delete", "Photo/1")]
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
            _ => throw new ArgumentException("Invalid HTTP method", nameof(httpMethod)),
        };

        // Act
        var response = await client.SendAsync(new HttpRequestMessage(method, url));

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var ticketResult = await client.PostAsJsonAsync("Ticket", _createTicketDto);
        var ticket = await ticketResult.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(ticket);

        var response = await client.PostAsJsonAsync("Photo", CreatePhotoDto(ticket.Id));
        var responseModel = await response.Content.ReadFromJsonAsync<Photo>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.GetAsync($"Photo/{responseModel.Id}");
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Photo>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(responseModel.Id, responseModel2.Id);
        Assert.AreEqual(ticket.Id, responseModel2.TicketId);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.GetAsync($"Photo/-1");
        var response2 = await client.GetAsync($"Photo/3834");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
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
        var response2 = await client.PostAsJsonAsync("Photo", photo);
        var responseModel = await response2.Content.ReadFromJsonAsync<Photo>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(photo.TicketId, responseModel.TicketId);
        Assert.AreEqual(photo.Name, responseModel.Name);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PostAsJsonAsync("Photo", new object { });
        var response2 = await client.PostAsJsonAsync("Photo", new Photo { TicketId = -1 });
        var response3 = await client.PostAsJsonAsync("Photo", new Photo { TicketId = 384 });
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
        var photoResponse = await client.PostAsJsonAsync("Photo", CreatePhotoDto(ticket.Id));
        var photo = await photoResponse.Content.ReadFromJsonAsync<Photo>();
        Assert.IsNotNull(photo);

        // Act
        photo.Name = expected;
        var result = await client.PutAsJsonAsync("Photo", new PhotoDto(photo));
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
        var response = await client.PutAsJsonAsync("Photo", new object { });
        var response2 = await client.PutAsJsonAsync("Photo", new PhotoDto { TicketId = -1 });
        var response3 = await client.PutAsJsonAsync("Photo", new PhotoDto { TicketId = 2734 });

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

        var response2 = await client.PostAsJsonAsync("Photo", CreatePhotoDto(responseModel.Id));
        var responseModel2 = await response2.Content.ReadFromJsonAsync<PhotoDto>();
        Assert.IsNotNull(responseModel2);

        // Act
        var response3 = await client.DeleteAsync($"Photo/{responseModel2.Id}");

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
        var response = await client.DeleteAsync($"Photo/-1");
        var response2 = await client.DeleteAsync($"Photo/2983");

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