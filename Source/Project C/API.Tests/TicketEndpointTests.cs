using System.Net;

using API.Tests.Utility;

namespace API.Tests;

[TestClass]
public class TicketEndpointTests : TestBase
{
    [TestMethod]
    public async Task Get_GetAllReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var response = await client.GetAsync("Ticket");
        Assert.IsNotNull(response);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
            if (tickets is not null)
                foreach (var ticket in tickets)
                    await client.DeleteAsync($"Ticket/{ticket.TicketId}");
        }

        // Act
        var response2 = await client.GetAsync("Ticket");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnOK()
    {
        // Arrange
        var client = _factory.CreateClient();
        for (int i = 1; i <= 2; i++)
        {
            await client.PostAsJsonAsync("Ticket", new Ticket
            {
                AdditionalNotes = $"Test{i}",
                TriedSolutions = $"Test{i}",
                Description = $"Test{i}",
                Priority = Priority.Critical,
            });
        }

        // Act
        var response = await client.GetAsync("Ticket");
        var responseModel = await response.Content.ReadFromJsonAsync<List<Ticket>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.IsTrue(responseModel.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnOk()
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
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.GetAsync($"Ticket/{responseModel.TicketId}");
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(responseModel.TicketId, responseModel2.TicketId);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"Ticket/3243");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"Ticket/-1");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllByTicketIdReturnNoContent()
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
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.GetAsync($"Ticket/{responseModel.TicketId}/photos");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllByTicketIdReturnOk()
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
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);
        for (int i = 1; i <= 2; i++)
        {
            var photo = new PhotoDto
            {
                TicketId = responseModel.TicketId,
                Name = $"Test{i}.png",
                Data = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
            };
            var result = await client.PostAsJsonAsync("Photo", photo);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }

        // Act
        var response2 = await client.GetAsync($"Ticket/{responseModel.TicketId}/photos");
        var photos = await response2.Content.ReadFromJsonAsync<List<Photo>>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(photos);
        Assert.IsTrue(photos.Any());
        Assert.IsTrue(photos.All(p => p.TicketId == responseModel.TicketId));
        Assert.IsTrue(photos.Count == 2);
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

        // Act
        var response = await client.PostAsJsonAsync("Ticket", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(model.AdditionalNotes, responseModel.AdditionalNotes);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("Ticket", new object { });

        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
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

        // Act
        responseModel.AdditionalNotes = expected;
        var response2 = await client.PutAsJsonAsync("Ticket", responseModel);
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
        var client = _factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync("Ticket", new object { });
        var response2 = await client.PutAsJsonAsync("Ticket", new Ticket { TicketId = -1 });

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
        var client = _factory.CreateClient();
        var model = new Ticket
        {
            AdditionalNotes = "Test",
            TriedSolutions = "Test",
            Description = "Test",
            Priority = Priority.Critical,
        };
        var response = await client.PostAsJsonAsync("Ticket", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.DeleteAsync($"Ticket/{responseModel.TicketId}");

        // Assert
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);

        // TODO: Ensure related photos are deleted as well
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"Ticket/-1");
        var response2 = await client.DeleteAsync($"Ticket/2983");

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }
}
