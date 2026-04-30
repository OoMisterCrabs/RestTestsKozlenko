using Allure.NUnit.Attributes;
using FluentAssertions;
using NUnit.Framework;
using RestTestsKozlenko.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestTestsKozlenko.Tests;

[TestFixture]
[AllureSuite("Users API")]
public class UsersTests : BaseTest
{
    [Test]
    [AllureFeature("GET /users")]
    [AllureDescription("Verifies that GET /users returns 200 OK with 10 users")]
    public async Task GetUsers_ShouldReturn200AndTenUsers()
    {
        var response = await Client.Get("/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        users.Should().HaveCount(10, because: "JSONPlaceholder provides exactly 10 users");
    }

    [Test]
    [AllureFeature("GET /users")]
    [AllureDescription("Verifies that every user has required fields: id, name, username, email")]
    public async Task GetUsers_EachUser_ShouldHaveRequiredFields()
    {
        var response = await Client.Get("/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        users.Should().NotBeNullOrEmpty();
        foreach (var user in users!)
        {
            user.Id.Should().BeGreaterThan(0);
            user.Name.Should().NotBeNullOrWhiteSpace(because: "name must not be empty");
            user.Username.Should().NotBeNullOrWhiteSpace(because: "username must not be empty");
            user.Email.Should().NotBeNullOrWhiteSpace(because: "email must not be empty");
        }
    }

    [Test]
    [AllureFeature("GET /users/{id}")]
    [AllureDescription("Verifies that a specific user can be fetched by ID")]
    public async Task GetUserById_ExistingId_ShouldReturnCorrectUser()
    {
        const int userId = 1;
        var response = await Client.Get($"/users/{userId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        user.Should().NotBeNull();
        user!.Id.Should().Be(userId);
    }

    [Test]
    [AllureFeature("GET /users/{id}")]
    [AllureDescription("Verifies that requesting a non-existent user returns 404")]
    public async Task GetUserById_NonExistentId_ShouldReturn404()
    {
        var response = await Client.Get("/users/9999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
