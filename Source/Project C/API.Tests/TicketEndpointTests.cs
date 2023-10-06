using System.Net;

using API.Tests.Utility;

namespace API.Tests;

[TestClass]
public class TicketEndpointTests : TestBase
{
    [DataTestMethod]
    [DataRow("/Ticket")]
    public async Task Get_EndpointReturnSuccess(string endpoint)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(endpoint);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task Post_CreateTicketReturnsCreated()
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
    public async Task Post_CreateTicketReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("Ticket", new object { });

        // Assert
        Assert.ThrowsException<HttpRequestException>(() => response.EnsureSuccessStatusCode());
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
