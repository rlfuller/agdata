using Newtonsoft.Json;
using RestSharp;
using System.Net;
using agdata.API.Models;
using RestSharp.Serializers;

namespace agdata.API.Tests
{
    internal class PostsTest : BaseTest
    {
        const string basePath = "/posts/";
        const int userId = 1;
        const int invalidPostId = 500;

        /// <summary>
        /// Private utility method to reuse for getting posts and doing some common assertions.
        /// </summary>
        /// <returns>A list of posts from the API</returns>
        private IList<Post> GetAllPosts()
        {
            RestRequest request = new RestRequest(basePath);
            RestResponse response = client.Get(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ContentType, Is.EqualTo(ContentType.Json));

            return JsonConvert.DeserializeObject<IList<Post>>(response.Content);
        }

        [Test]
        public void TestGetPosts()
        {
            IList<Post> posts = GetAllPosts();
            Assert.That(posts, Is.Not.Empty);
        }

        /// <summary>Validate that requesting a Post Resource with a valid PostId is successful</summary>
        [Test]
        public void GetSinglePostResourceWithValidPostId()
        {
            IList<Post> posts = GetAllPosts();
            
            // No need to repeat the assertion for the count. We already assert that in "TestGetPosts".
            if (posts.Count == 0)
            {
                return;
            }

            RestRequest request = new RestRequest($"{basePath}{posts.First().Id}");
            RestResponse response = client.Get(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ContentType, Is.EqualTo(ContentType.Json));

            Post body = JsonConvert.DeserializeObject<Post>(response.Content);

            Assert.That(body.Id, Is.EqualTo(posts.First().Id));
        }

        /// <summary>
        /// Validate that requesting a Post Resource with an invalid PostId fails
        /// </summary>
        [Test]
        public void PostResourceWithInvalidPostIdTest()
        {
            RestRequest request = new RestRequest($"{basePath}{invalidPostId}", Method.Get);

            RestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            Post body = JsonConvert.DeserializeObject<Post>(response.Content);

            Assert.That(body.Id, Is.EqualTo(0) );
        }

        /// <summary>
        /// Utility function that creates a resource of type post. To be used as setup for crud tests
        /// </summary>
        /// <param name="title">Title of the post</param>
        /// <param name="body">Body of the post</param>
        /// <param name="userId">UserId of the author of the post</param>
        /// <returns>Post object</returns>
        Post CreatePost(string title, string body, int userId)
        {
            var post = new
            {
                Title = title,
                Body = body,
                UserId = userId
            };

            RestRequest request = new RestRequest(basePath);
            request.AddBody(post, ContentType.Json);

            RestResponse response = client.Post(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            return JsonConvert.DeserializeObject<Post>(response.Content);
        }

        /// <summary>
        /// Validate that creating a post with valid data is succesful.  For the third test, we will assume that an empty body is valid. 
        /// </summary>
        /// <param name="title">post resource Title</param>
        /// <param name="body">post resource Body</param>
        /// <param name="userId">post resource UserId</param>
        [TestCase("A title", "The body", userId, TestName = "Basic data")]
        [TestCase("Title ååå", "The body ååå", userId, TestName = "Unicode data")]
        [TestCase("A title", "", userId, TestName = "Empty body")]
        public void CreatePostResourceWithValidDataIsSuccessful(string title, string body, int userId)
        {
            Post created = CreatePost(title, body, userId);

            Assert.That(created.Id, Is.EqualTo(101));
            Assert.That(created.Title, Is.EqualTo(title));
            Assert.That(created.Body, Is.EqualTo(body));
            Assert.That(created.UserId, Is.EqualTo(userId));
        }

        /// <summary>
        /// Validate that creating a post resouce with invalid data is not successful. 
        /// </summary>
        /// <param name="title">Post Resource Title</param>
        /// <param name="body">Post Resource Body</param>
        /// <param name="userId">Post Resource UserId</param>
        [TestCase("", "The body", userId, TestName = "Empty title")]
        [TestCase(null, "The body", userId, TestName = "Null title")]
        [TestCase("A title", null, userId, TestName = "Null body")]
        [TestCase("A title", "The body", null, TestName = "Null userId")]
        [TestCase("A title", "The body", -1, TestName = "Negative userId")]
        [TestCase("A title", "The body", long.MaxValue, TestName = "userId too large")]
        public void CreatePostResourceWithInvalidDataIsNotSuccessful(string title, string body, long? userId)
        {
            var post = new
            {
                Title = title,
                Body = body,
                UserId = userId
            };

            RestRequest request = new RestRequest(basePath);
            request.AddBody(post, ContentType.Json);

            RestResponse response = client.Post(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        /// <summary>
        /// Validate that a Post request with a bad content type will fail
        /// </summary>
        [Test]
        public void PostBadContentType()
        {
            RestRequest request = new RestRequest(basePath);
            request.AddBody("foo", ContentType.Binary);

            RestResponse response = client.Post(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        /// <summary>
        /// Validate that a Put request with valid data succeeds.
        /// </summary>
        [Test]
        public void PutRequestWithValidDataIsSuccessful()
        {
            Post existing = CreatePost("foo", "bar", userId);

            var edited = new
            {
                Title = $"{existing.Body} edited",
                Body = $"{existing.Body} edited",
                UserId = existing.UserId + 1
            };

            RestRequest request = new RestRequest($"{basePath}{existing.Id}");
            request.AddBody(edited, ContentType.Json);
            RestResponse response = client.Put(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Post updated = JsonConvert.DeserializeObject<Post>(response.Content);

            Assert.That(updated.Id, Is.EqualTo(existing.Id));
            Assert.That(updated.Title, Is.EqualTo(edited.Title));
            Assert.That(updated.Body, Is.EqualTo(edited.Body));
            Assert.That(updated.UserId, Is.EqualTo(edited.UserId));
        }

        [Test]
        public void DeleteIsSuccessfulWithValidData()
        {
            //create a new post resource
            Post existing = CreatePost("foo", "Bar", userId);

            //Delete the resource that we just created
            RestRequest request = new RestRequest($"{basePath}{existing.Id}");
            RestResponse response = client.Delete(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            //Now try to get the resource and verify that a 404 is returned
            request = new RestRequest($"{basePath}{existing.Id}");

            response = client.Get(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void DeleteIsNotSuccessfulWithInvalidData()
        {
            RestRequest request = new RestRequest($"{basePath}{int.MaxValue}");
            RestResponse response = client.Delete(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
