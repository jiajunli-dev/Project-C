namespace API.Tests;

[TestClass]
public class PhotoEndpointTests : TestBase
{
    private const string _endpoint = "Photo";

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
    public override async Task Endpoints_EnsureAuthorization(string method,
                                                string endpoint,
                                                string? role = null,
                                                bool isAuthorized = false)
        => await base.Endpoints_EnsureAuthorization(method,
                                                    endpoint,
                                                    role,
                                                    isAuthorized);

    [TestMethod]
    public async Task Get_GetByIdReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var expectedTicket = await CreateTicketInDb(1);
        var expectedPhoto = await CreatePhotoInDb(expectedTicket.Id, 1);

        // Act
        var result = await client.GetAsync($"{_endpoint}/{expectedPhoto.Id}");
        var resultPhoto = await result.Content.ReadFromJsonAsync<Photo>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(resultPhoto);
        Assert.AreEqual(expectedTicket.Id, resultPhoto.TicketId);
        Assert.AreEqual(expectedPhoto.Id, resultPhoto.Id);
        Assert.AreEqual(expectedPhoto.CreatedBy, resultPhoto.CreatedBy);
        Assert.AreEqual(expectedPhoto.CreatedAt, resultPhoto.CreatedAt);
        Assert.AreEqual(expectedPhoto.UpdatedAt, resultPhoto.UpdatedAt);
        Assert.AreEqual(expectedPhoto.UpdatedBy, resultPhoto.UpdatedBy);
        Assert.AreEqual(expectedPhoto.Name, resultPhoto.Name);
        CollectionAssert.AreEqual(expectedPhoto.Data, resultPhoto.Data);
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
        var expectedTicket = await CreateTicketInDb(2);
        var expectedPhoto = CreatePhotoDto(expectedTicket.Id, 2);

        // Act
        var result = await client.PostAsJsonAsync(_endpoint, expectedPhoto);
        result.EnsureSuccessStatusCode();
        var resultPhoto = await result.Content.ReadFromJsonAsync<Photo>();

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        Assert.IsNotNull(resultPhoto);
        Assert.AreEqual(expectedTicket.Id, resultPhoto.TicketId);
        Assert.AreEqual(expectedPhoto.CreatedBy, resultPhoto.CreatedBy);
        Assert.AreEqual(expectedPhoto.CreatedBy, resultPhoto.UpdatedBy);
        Assert.AreEqual(expectedPhoto.Name, resultPhoto.Name);
        CollectionAssert.AreEqual(Convert.FromBase64String(expectedPhoto.Data), resultPhoto.Data);
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
        var expectedTicket = await CreateTicketInDb(3);
        var expectedPhoto = await CreatePhotoInDb(expectedTicket.Id, 3);
        var expected = "This is a changed value";

        // Act
        expectedPhoto.Name = expected;
        var result = await client.PutAsJsonAsync(_endpoint, new PhotoDto(expectedPhoto));
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
        var expectedTicket = await CreateTicketInDb(4);
        var expectedPhoto = await CreatePhotoInDb(expectedTicket.Id, 4);

        // Act
        var result = await client.DeleteAsync($"{_endpoint}/{expectedPhoto.Id}");

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
        var response2 = await client.DeleteAsync($"{_endpoint}/2983");

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    private static CreatePhotoDto CreatePhotoDto(int ticketId, int i) => new()
    {
        CreatedBy = $"Test_{i}",
        TicketId = ticketId,
        Name = $"Test{i}.png",
        Data = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
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

    private async Task<Photo> CreatePhotoInDb(int ticketId, int i)
    {
        var model = CreatePhotoDto(ticketId, i);
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
        var resultModel = await result.Content.ReadFromJsonAsync<Photo>();
        if (resultModel is null)
            Assert.Inconclusive("Failed to create photo");

        return resultModel;
    }
}