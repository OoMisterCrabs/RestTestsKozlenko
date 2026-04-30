using Allure.NUnit.Attributes;
using FluentAssertions;
using NUnit.Framework;
using RestTestsKozlenko.Models;
using System.Net;
using System.Text.Json;

namespace RestTestsKozlenko.Tests;

[TestFixture]
[AllureSuite("Posts API")]
public class PostsTests : BaseTest
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    // ─────────────────────────────────────────
    // GET /posts
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("GET /posts")]
    [AllureDescription("Verifies that GET /posts returns HTTP 200 OK")]
    public async Task GetPosts_ShouldReturn200OK()
    {
        var response = await Client.Get("/posts");

        response.StatusCode.Should().Be(HttpStatusCode.OK,
            because: "the API should return 200 OK for a valid GET /posts request");
    }

    [Test]
    [AllureFeature("GET /posts")]
    [AllureDescription("Verifies that GET /posts returns exactly 100 posts")]
    public async Task GetPosts_ShouldReturn100Posts()
    {
        var response = await Client.Get("/posts");

        var content = await response.Content.ReadAsStringAsync();
        var posts = JsonSerializer.Deserialize<List<Post>>(content, _jsonOptions);

        posts.Should().NotBeNull(because: "response body should be deserialized");
        posts!.Should().HaveCount(100,
            because: "JSONPlaceholder provides exactly 100 posts");
    }

    [Test]
    [AllureFeature("GET /posts")]
    [AllureDescription("Verifies that every post has the required fields: id, userId, title, body")]
    public async Task GetPosts_EachPost_ShouldHaveRequiredFields()
    {
        var response = await Client.Get("/posts");
        var content = await response.Content.ReadAsStringAsync();
        var posts = JsonSerializer.Deserialize<List<Post>>(content, _jsonOptions);

        posts.Should().NotBeNullOrEmpty();
        foreach (var post in posts!)
        {
            post.Id.Should().BeGreaterThan(0, because: "id must be a positive integer");
            post.UserId.Should().BeGreaterThan(0, because: "userId must be a positive integer");
            post.Title.Should().NotBeNullOrWhiteSpace(because: "title must not be empty");
            post.Body.Should().NotBeNullOrWhiteSpace(because: "body must not be empty");
        }
    }

    // ─────────────────────────────────────────
    // GET /posts/{id}
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("GET /posts/{id}")]
    [AllureDescription("Verifies that a specific post can be fetched by ID and has correct data")]
    public async Task GetPostById_ExistingId_ShouldReturnCorrectPost()
    {
        const int postId = 1;
        var response = await Client.Get($"/posts/{postId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK,
            because: "post with id=1 exists");

        var content = await response.Content.ReadAsStringAsync();
        var post = JsonSerializer.Deserialize<Post>(content, _jsonOptions);

        post.Should().NotBeNull();
        post!.Id.Should().Be(postId, because: "returned post id must match requested id");
        post.UserId.Should().BeGreaterThan(0);
        post.Title.Should().NotBeNullOrWhiteSpace();
        post.Body.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    [AllureFeature("GET /posts/{id}")]
    [AllureDescription("Verifies that requesting a non-existent post ID returns 404 Not Found")]
    public async Task GetPostById_NonExistentId_ShouldReturn404()
    {
        const int nonExistentId = 9999;
        var response = await Client.Get($"/posts/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound,
            because: "post with id=9999 does not exist in JSONPlaceholder");
    }

    // ─────────────────────────────────────────
    // POST /posts
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("POST /posts")]
    [AllureDescription("Verifies that creating a new post returns 201 Created")]
    public async Task CreatePost_ShouldReturn201Created()
    {
        var newPost = new CreatePostRequest
        {
            UserId = 1,
            Title = "Test Post Title",
            Body = "Test post body content for automated test"
        };

        var response = await Client.Post("/posts", newPost);

        response.StatusCode.Should().Be(HttpStatusCode.Created,
            because: "a successful POST should return 201 Created");
    }

    [Test]
    [AllureFeature("POST /posts")]
    [AllureDescription("Verifies that the response body of a POST reflects the submitted data")]
    public async Task CreatePost_ResponseBody_ShouldContainSubmittedData()
    {
        var newPost = new CreatePostRequest
        {
            UserId = 5,
            Title = "Automation Test Post",
            Body = "Created by automated test suite"
        };

        var response = await Client.Post("/posts", newPost);
        var content = await response.Content.ReadAsStringAsync();
        var post = JsonSerializer.Deserialize<Post>(content, _jsonOptions);

        post.Should().NotBeNull();
        post!.UserId.Should().Be(newPost.UserId,
            because: "userId in response must match request");
        post.Title.Should().Be(newPost.Title,
            because: "title in response must match request");
        post.Body.Should().Be(newPost.Body,
            because: "body in response must match request");
        post.Id.Should().BeGreaterThan(0,
            because: "server should assign a new id to the created post");
    }

    // ─────────────────────────────────────────
    // PUT /posts/{id}
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("PUT /posts/{id}")]
    [AllureDescription("Verifies that updating an existing post returns 200 and reflects the changes")]
    public async Task UpdatePost_ShouldReturn200AndUpdatedData()
    {
        const int postId = 1;
        var updatedPost = new Post
        {
            Id = postId,
            UserId = 1,
            Title = "Updated Title by Automation",
            Body = "Updated body content by automated test"
        };

        var response = await Client.Put($"/posts/{postId}", updatedPost);

        response.StatusCode.Should().Be(HttpStatusCode.OK,
            because: "a successful PUT should return 200 OK");

        var content = await response.Content.ReadAsStringAsync();
        var post = JsonSerializer.Deserialize<Post>(content, _jsonOptions);

        post.Should().NotBeNull();
        post!.Id.Should().Be(postId);
        post.Title.Should().Be(updatedPost.Title,
            because: "title should reflect the updated value");
        post.Body.Should().Be(updatedPost.Body,
            because: "body should reflect the updated value");
    }

    // ─────────────────────────────────────────
    // DELETE /posts/{id}
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("DELETE /posts/{id}")]
    [AllureDescription("Verifies that deleting an existing post returns 200 OK")]
    public async Task DeletePost_ExistingId_ShouldReturn200()
    {
        const int postId = 1;
        var response = await Client.Delete($"/posts/{postId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK,
            because: "JSONPlaceholder returns 200 OK on successful DELETE");
    }

    // ─────────────────────────────────────────
    // BONUS: Filter posts by userId
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("GET /posts")]
    [AllureDescription("Verifies that posts can be filtered by userId query parameter")]
    public async Task GetPosts_FilteredByUserId_ShouldReturnOnlyMatchingPosts()
    {
        const int userId = 1;
        var response = await Client.Get($"/posts?userId={userId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var posts = JsonSerializer.Deserialize<List<Post>>(content, _jsonOptions);

        posts.Should().NotBeNullOrEmpty(because: "user 1 has posts");
        posts!.Should().AllSatisfy(p =>
            p.UserId.Should().Be(userId, because: "all returned posts must belong to the requested userId"));
    }

    // ─────────────────────────────────────────
    // BONUS: Nested resource
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("GET /posts/{id}/comments")]
    [AllureDescription("Verifies that nested comments for a post are returned correctly")]
    public async Task GetPostComments_ExistingPost_ShouldReturnComments()
    {
        const int postId = 1;
        var response = await Client.Get($"/posts/{postId}/comments");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var comments = JsonSerializer.Deserialize<List<Comment>>(content, _jsonOptions);

        comments.Should().NotBeNullOrEmpty(because: "post 1 has comments");
        comments!.Should().AllSatisfy(c =>
        {
            c.PostId.Should().Be(postId, because: "all comments must belong to the requested post");
            c.Id.Should().BeGreaterThan(0);
            c.Email.Should().NotBeNullOrWhiteSpace(because: "email field must be present");
        });
    }

    // ─────────────────────────────────────────
    // BONUS: Response time validation
    // ─────────────────────────────────────────

    [Test]
    [AllureFeature("Performance")]
    [AllureDescription("Verifies that GET /posts responds within 3 seconds")]
    public async Task GetPosts_ResponseTime_ShouldBeWithinAcceptableLimit()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var response = await Client.Get("/posts");

        stopwatch.Stop();
        Logger.Log($"[PERF] Response time: {stopwatch.ElapsedMilliseconds}ms");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000,
            because: "API response should arrive within 3 seconds");
    }
}
