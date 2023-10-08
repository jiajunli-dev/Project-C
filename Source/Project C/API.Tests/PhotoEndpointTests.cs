using System.Net;

using API.Tests.Utility;

namespace API.Tests;

[TestClass]
public class PhotoEndpointTests : TestBase
{
    [TestMethod]
    public async Task Get_GetByIdReturnOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var ticket = new Ticket
        {
            AdditionalNotes = "Test",
            Description = "Test",
            TriedSolutions = "Test",
            Priority = Priority.High,
            UserId = 1
        };
        var ticketResult = await client.PostAsJsonAsync("Ticket", ticket);
        ticket = await ticketResult.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(ticket);

        var model = new Photo
        {
            TicketId = ticket.TicketId,
            Name = "Test.png",
            Data = "    "u8.ToArray()
        };

        var response = await client.PostAsJsonAsync("Photo", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Photo>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.GetAsync($"Photo/{responseModel.PhotoId}");
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Photo>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(responseModel.PhotoId, responseModel2.PhotoId);
        Assert.AreEqual(ticket.TicketId, responseModel2.TicketId);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

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
        var client = _factory.CreateClient();
        var model = new Ticket
        {
            AdditionalNotes = "Test",
            TriedSolutions = "Test",
            Description = "Test",
            Priority = Priority.Critical,
        };
        var response = await client.PostAsJsonAsync("Ticket", model);
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(ticket);
        var photo = new Photo
        {
            TicketId = ticket.TicketId,
            Name = "Test.png",
            Data = "    "u8.ToArray()
        };

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
        var client = _factory.CreateClient();

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
        var client = _factory.CreateClient();
        var model = new Ticket
        {
            AdditionalNotes = "Test",
            TriedSolutions = "Test",
            Description = "Test",
            Priority = Priority.Critical,
        };
        var expected = "This is a changed value";
        var response = await client.PostAsJsonAsync("Ticket", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);
        var photo = PhotoDto.FromPhoto(new Photo
        {
            TicketId = responseModel.TicketId,
            Name = "Test.png",
            Data = "    "u8.ToArray()
        });
        var response2 = await client.PostAsJsonAsync("Photo", photo);
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Photo>();
        Assert.IsNotNull(responseModel2);

        // Act
        responseModel2.Name = expected;
        var response3 = await client.PutAsJsonAsync("Photo", responseModel2);
        var responseModel3 = await response3.Content.ReadFromJsonAsync<Photo>();

        // Assert
        response3.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
        Assert.IsNotNull(responseModel3);
        Assert.AreEqual(expected, responseModel3.Name);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

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
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("Ticket", new Ticket
        {
            AdditionalNotes = "Test",
            TriedSolutions = "Test",
            Description = "Test",
            Priority = Priority.Critical,
        });
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

        var response2 = await client.PostAsJsonAsync("Photo", PhotoDto.FromPhoto(new Photo
        {
            TicketId = responseModel.TicketId,
            Name = "Test.png",
            Data = "    "u8.ToArray()
        }));
        var responseModel2 = await response2.Content.ReadFromJsonAsync<PhotoDto>();
        Assert.IsNotNull(responseModel2);

        // Act
        var response3 = await client.DeleteAsync($"Photo/{responseModel2.PhotoId}");

        // Assert
        Assert.IsNotNull(response3);
        Assert.AreEqual(HttpStatusCode.NoContent, response3.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"Photo/-1");
        var response2 = await client.DeleteAsync($"Photo/2983");

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }
}