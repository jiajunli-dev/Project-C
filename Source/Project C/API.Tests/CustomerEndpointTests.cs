using System;
using System.Net;

using API.Tests.Utility;
using API.Utility;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Tests;

[TestClass]
public class CustomerEndpointTests : TestBase
{
    [TestMethod]
    [DataRow("Get", "Customer")]
    [DataRow("Post", "Customer")]
    [DataRow("Put", "Customer")]
    [DataRow("Get", "Customer/1")]
    [DataRow("Delete", "Customer/1")]
    public async Task Endpoints_ReturnUnauthorized(string httpMethod, string url)
    {
        //Arrange
        var client = CreateClient();

        var method = httpMethod switch
        {
            "Get" => HttpMethod.Get,
            "Post" => HttpMethod.Post,
            "Put" => HttpMethod.Put,
            "Delete" => HttpMethod.Delete,
            _ => throw new ArgumentException("Invalid HTTP method", nameof(httpMethod))
        };

        // Act
        var response = await client.SendAsync(new HttpRequestMessage(method, url));

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    [DataRow("Get", "Customer", Roles.ADMIN, HttpStatusCode.NoContent)]
    [DataRow("Get", "Customer", Roles.CUSTOMER, HttpStatusCode.Forbidden)]
    [DataRow("Get", "Customer", Roles.EMPLOYEE, HttpStatusCode.NoContent)]
    
    [DataRow("Get", "Customer/1", Roles.ADMIN, HttpStatusCode.BadRequest)]
    [DataRow("Get", "Customer/1", Roles.CUSTOMER, HttpStatusCode.BadRequest)]
    [DataRow("Get", "Customer/1", Roles.EMPLOYEE, HttpStatusCode.BadRequest)]
    
    // TODO: Fix test post
    /*[DataRow("Post", "Customer", Roles.ADMIN, HttpStatusCode.BadRequest)]
    [DataRow("Post", "Customer", Roles.CUSTOMER, HttpStatusCode.BadRequest)]
    [DataRow("Post", "Customer", Roles.EMPLOYEE, HttpStatusCode.BadRequest)]*/
    
    // TODO: Fix test put
    /*[DataRow("Put", "Customer", Roles.ADMIN, HttpStatusCode.BadRequest)]
    [DataRow("Put", "Customer", Roles.CUSTOMER, HttpStatusCode.BadRequest)]
    [DataRow("Put", "Customer", Roles.EMPLOYEE, HttpStatusCode.BadGateway)]*/

    [DataRow("Delete", "Customer/1", Roles.CUSTOMER, HttpStatusCode.Forbidden)]
    [DataRow("Delete", "Customer/1", Roles.EMPLOYEE, HttpStatusCode.Forbidden)]
    [DataRow("Delete", "Customer/1", Roles.ADMIN, HttpStatusCode.BadRequest)]
    public async Task Endpoints_EnsureAuthorizationConfiguration(string method, string endpoint, string role, HttpStatusCode expected)
    {
        // Arrange
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

        // Act
        var result = await client.SendAsync(new HttpRequestMessage(httpMethod, endpoint));

        // Assert
        Assert.AreEqual(expected, result.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnNoContent()
    {
        // Arrange
        var client = CreateAdminClient();
        var response = await client.GetAsync("Customer");
        Assert.IsNotNull(response);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();
            if (customers != null)
                foreach (var customer in customers)
                    await client.DeleteAsync($"Customer/{customer.UserId}");
        }

        // Act
        var response2 = await client.GetAsync("Customer");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Get_GetAllReturnOK()
    {
        // Arrange
        var client = CreateAdminClient();
        for (int i = 1; i <= 2; i++)
        {
            await client.PostAsJsonAsync("Customer", new Customer
            {
                UserId = i,
                PhoneNumber = "1234567890",
                CompanyName = $"Test{i} Company",
                CompanyPhoneNumber = "1234567890",
                DepartmentName = $"Test{i} Department"
            });
        }

        // Act
        var response = await client.GetAsync("Customer");
        var model = await response.Content.ReadFromJsonAsync<List<Customer>>();
        

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(model);
        Assert.IsTrue(model.Any());
    }

    [TestMethod]
    public async Task Get_GetByIdReturnOk()
    {
        // Arrange
        var client = CreateAdminClient();

        var model = new Customer
        {
            PhoneNumber = "1234567890",
            CompanyName = "Test Company",
            CompanyPhoneNumber = "1234567890",
            DepartmentName = "Test Department"
        };
        var response = await client.PostAsJsonAsync("Customer", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.GetAsync($"Customer/{responseModel.UserId}");
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Customer>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(responseModel.UserId, responseModel2.UserId);
    }

    [TestMethod]
    public async Task Get_GetByIdReturnNoContent()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.GetAsync("Customer/-1");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Post_CreateReturnsCreated()
    {
        // Arrange
        var client = CreateAdminClient();
        var model = new Customer
        {
            PhoneNumber = "1234567890",
            CompanyName = "Test Company",
            CompanyPhoneNumber = "1234567890",
            DepartmentName = "Test Department"
        };

        // Act
        var response = await client.PostAsJsonAsync("Customer", model);
        var responseModel = await response.Content.ReadFromJsonAsync<Customer>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(model.CompanyName, responseModel.CompanyName);
    }

    [TestMethod]
    public async Task Post_CreateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();
        
        // Act
        var response = await client.PostAsJsonAsync("Customer", new Customer { });

        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsOk()
    {
        // Arrange
        var client = CreateAdminClient();
        var model = new Customer
        {
            PhoneNumber = "1234567890",
            CompanyName = "Test Company",
            CompanyPhoneNumber = "1234567890",
            DepartmentName = "Test Department"
        };  
        var response = await client.PostAsJsonAsync("Customer", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.IsNotNull(responseModel);

        // Act
        var expected = "This is a test";
        responseModel.CompanyName = expected;
        var response2 = await client.PutAsJsonAsync("Customer", responseModel);
        var responseModel2 = await response2.Content.ReadFromJsonAsync<Customer>();

        // Assert
        response2.EnsureSuccessStatusCode();
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        Assert.IsNotNull(responseModel2);
        Assert.AreEqual(expected, responseModel2.CompanyName);
    }

    [TestMethod]
    public async Task Put_UpdateReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.PutAsJsonAsync("Customer", new Customer { });
        var response2 = await client.PutAsJsonAsync("Customer", new Customer { UserId = -1 });
    
        // Assert
        Assert.ThrowsException<HttpRequestException>(response.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.ThrowsException<HttpRequestException>(response2.EnsureSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsNoContent()
    {
        var client = CreateAdminClient();
        var model = new Customer
        {
            PhoneNumber = "1234567890",
            CompanyName = "Test Company",
            CompanyPhoneNumber = "1234567890",
            DepartmentName = "Test Department"
        };
        var response = await client.PostAsJsonAsync("Customer", model);
        response.EnsureSuccessStatusCode();
        var responseModel = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.IsNotNull(responseModel);

        // Act
        var response2 = await client.DeleteAsync($"Customer/{responseModel.UserId}");

        // Assert
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [TestMethod]
    public async Task Delete_DeleteReturnsBadRequest()
    {
        // Arrange
        var client = CreateAdminClient();

        // Act
        var response = await client.DeleteAsync("Customer/-1");
        var response2 = await client.DeleteAsync("Customer/1000");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response2);
        Assert.AreEqual(HttpStatusCode.BadRequest, response2.StatusCode);
    }
}
