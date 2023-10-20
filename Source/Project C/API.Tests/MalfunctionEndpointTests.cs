namespace API.Tests;

[TestClass]
public class MalfunctionEndpointTests : TestBase
{
    [TestMethod]
    [DataRow("Get", "Malfunction")]
    [DataRow("Post", "Malfunction")]
    [DataRow("Put", "Malfunction")]
    [DataRow("Get", "Malfunction/1")]
    [DataRow("Delete", "Ticket/1")]
    public async Task Endpoints_ReturnUnauthorized(string httpMethod, string url)
    {
        var client = CreateClient();

        var method = httpMethod switch
        {
            "Get" => HttpMethod.Get,
            "Post" => HttpMethod.Post,
            "Put" => HttpMethod.Put,
            "Delete" => HttpMethod.Delete,
            _ => throw new ArgumentException("Invalid HTTP method", nameof(httpMethod)),
        };

        var response = await client.SendAsync(new HttpRequestMessage(method, url));

        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    [DataRow("Get", "Malfunction", Roles.ADMIN, HttpStatusCode.OK)]
    [DataRow("Get", "Malfunction", Roles.CUSTOMER, HttpStatusCode.OK)]
    [DataRow("Get", "Malfunction", Roles.EMPLOYEE, HttpStatusCode.OK)]
    public async Task Endpoints_EnsureAuthorizationConfiguration(string method, string endpoint, string role, HttpStatusCode expected)
    {
        var client = role switch
        {
            Roles.ADMIN => CreateAdminClient(),
            Roles.EMPLOYEE => CreateEmployeeClient(),
            Roles.CUSTOMER => CreateCustomerClient(),
            _ => throw new ArgumentException("Invalid role", nameof(role)),
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

        Assert.AreEqual(expected, result.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnNoContent()
    {
        var client = CreateAdminClient();
        var response = await client.GetAsync("Malfunction");

        Assert.IsNotNull(response);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var malfunctions = await response.Content.ReadFromJsonAsync<List<Malfunction>>();
            if (malfunctions is not null)
                foreach (var malfunction in malfunctions)
                    await client.DeleteAsync($"Malfunction/{malfunction.Id}");
        }

        var response2 = await client.GetAsync("Malfunction");

        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnOK()
    {
        var client = CreateAdminClient();
        for (int i = 1; i <= 2; i++)
        {
            await client.PostAsJsonAsync("Malfunction", new Malfunction
            {
                Description = $"Test{i}",
                Solution = $"Test{i}",
                Priority = Priority.Critical,
            });
        }

        var response = await client.GetAsync("Malfunction");
        var responseModel = await response.Content.ReadFromJsonAsync<List<Malfunction>>();

        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.IsTrue(responseModel.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnOk()
    {
        var client = CreateAdminClient();

        var model = new Malfunction
        {
            Description = $"Test",
            Solution = $"Test",
            Priority = Priority.Critical,
        };
        var response = await client.PostAsJsonAsync("Malfunction", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(responseModel);

        var response2 = await client.GetAsync($"Malfunction/{responseModel.Id}");
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Malfunction>();

        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(responseModel.Id, responseModel2.Id);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.GetAsync($"Malfunction/-1");

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllByMalfunctionIdReturnNoContent()
    {
        var client = CreateAdminClient();
        var model = new Malfunction
        {
            Description = $"Test",
            Solution = $"Test",
            Priority = Priority.Critical,
        };
        var response = await client.PostAsJsonAsync("Malfunction", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(responseModel);

        var response2 = await client.GetAsync($"Malfunction/{responseModel.Id}");

        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllByMalfunctionIdReturnOk()
    {
        var client = CreateAdminClient();
        var model = new Malfunction
        {
            Description = $"Test",
            Solution = $"Test",
            Priority = Priority.Critical,
        };
        var response = await client.PostAsJsonAsync("Malfunction", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(responseModel);

        var response2 = await client.GetAsync($"Malfunction/{responseModel.Id}");
        var malfunctions = await response2.Content.ReadFromJsonAsync<List<Malfunction>>();

        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(malfunctions);
        Assert.IsTrue(malfunctions.Any());
        Assert.IsTrue(malfunctions.All(p => p.Id == responseModel.Id));
        Assert.IsTrue(malfunctions.Count == 2);
    }

    [TestMethod]
    public async Task Post_CreateReturnsCreated()
    {
        var client = CreateAdminClient();
        var model = new Malfunction
        {
            Description = $"Test",
            Solution = $"Test",
            Priority = Priority.Critical,
        };

        var response = await client.PostAsJsonAsync("Malfunction", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();

        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(model.Description, responseModel.Description);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.PostAsJsonAsync("Malfunction", new object { });

        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOk()
    {
        var client = CreateAdminClient();
        var model = new Malfunction
        {
            Description = $"Test",
            Solution = $"Test",
            Priority = Priority.Critical,
        };
        var expected = "This is a changed value";
        var response = await client.PostAsJsonAsync("Malfunction", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(responseModel);

        responseModel.Description = expected;
        var response2 = await client.PutAsJsonAsync("Malfunction", responseModel);
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Malfunction>();

        response2.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(expected, responseModel2.Description);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.PutAsJsonAsync("Malfunction", new object { });
        var response2 = await client.PutAsJsonAsync("Malfunction", new Malfunction { Id = -1 });

        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.ThrowsException<HttpRequestException>(response2.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsNoContent()
    {
        var client = CreateAdminClient();
        var model = new Malfunction
        {
            Description = $"Test",
            Solution = $"Test",
            Priority = Priority.Critical,
        };
        var response = await client.PostAsJsonAsync("Malfunction", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Malfunction>();
        Assert.IsNotNull(responseModel);

        var response2 = await client.DeleteAsync($"Malfunction/{responseModel.Id}");

        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        var client = CreateAdminClient();

        var response = await client.DeleteAsync($"Malfunction/-1");
        var response2 = await client.DeleteAsync($"Malfunction/2222");

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }
}

