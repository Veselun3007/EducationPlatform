using CourseContent.Core.DTO.Requests.AssignmentDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Tests.Base;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using System.Net;
using System.Text;
using CourseContent.Tests.Config;

namespace CourseContent.Tests.ControllerTests
{
    public class AssignmentControllerTests(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        : BaseIntegrationTest(factory)
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;
        private const string baseUrl = "https://localhost:5002/api/assignment";
        const int courseId = 1;

        [Fact]
        public async Task CreateAssignment_ValidAssignment_ReturnsOk()
        {
            // Arrange

            var assignmentDto = new AssignmentDTO
            {
                CourseId = 1,
                TopicId = 1,
                AssignmentName = "New test task #1",
                AssignmentDescription = "New test description 1",
                AssignmentDatePublication = DateTime.UtcNow,
                AssignmentDeadline = DateTime.UtcNow.AddDays(7),
                MaxMark = 10,
                MinMark = 5,
                IsRequired = true,
                AssignmentLinks = ["https://example.com/link1", "https://example.com/link2"],
            };

            var assignmentFormData = new MultipartFormDataContent
             {
                 { new StringContent(assignmentDto.CourseId.ToString()), "CourseId" },
                 { new StringContent(assignmentDto.TopicId.ToString()!), "TopicId" },
                 { new StringContent(assignmentDto.AssignmentName), "AssignmentName" },
                 { new StringContent(assignmentDto.AssignmentDescription), "AssignmentDescription" },
                 { new StringContent(assignmentDto.MaxMark.ToString()), "MaxMark" },
                 { new StringContent(assignmentDto.MinMark.ToString()), "MinMark" },
                 { new StringContent(assignmentDto.IsRequired.ToString()), "IsRequired" },
                 { new StringContent(assignmentDto.AssignmentDeadline.ToString("o")), "AssignmentDeadline" },
             };
            foreach (var link in assignmentDto.AssignmentLinks)
            {
                assignmentFormData.Add(new StringContent(link), $"AssignmentLinks[{assignmentDto.AssignmentLinks.IndexOf(link)}]");
            }           
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.PostAsync($"{baseUrl}/create", assignmentFormData);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var createdAssignment = jsonObject?.TryGetValue("result", out var resultToken) == true
                       ? resultToken.ToObject<AssignmentOutDTO>() : null;

            Assert.NotNull(createdAssignment);
            _testOutputHelper.WriteLine
                (
                    $"Id:\t{createdAssignment.Id}\n" +
                    $"Name:\t{createdAssignment.AssignmentName}\n" +
                    $"Description:\t{createdAssignment.AssignmentDescription}\n" +
                    $"Date Publication:\t{createdAssignment.AssignmentDatePublication}\n" +
                    $"Deadline:\t{createdAssignment.AssignmentDeadline}\n" +
                    $"Is Edited:\t{createdAssignment.IsEdited}\n"
                );

        }

        [Fact]
        public async Task GetAllAssignment_ReturnsExpectedAssignments()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.GetAsync($"{baseUrl}/getAll/{courseId}");

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var assignmentsJson = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(assignmentsJson))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonArray = JsonConvert.DeserializeObject<JArray>(assignmentsJson);
            if (jsonArray is null)
            {
                Assert.Fail("Response content is empty");
            }

            var assignments = jsonArray.ToObject<List<AssignmentOutDTO>>();

            Assert.NotNull(assignments);
            Assert.NotEmpty(assignments);
            _testOutputHelper.WriteLine($"Row count:\t {assignments.Count}");
            foreach(AssignmentOutDTO assignment in assignments)
            {
                _testOutputHelper.WriteLine(assignment.AssignmentName);
            }
        }

        [Fact]
        public async Task UpdateAssignment_ValidAssignment_ReturnsOk()
        {
            // Arrange
            var assignmentDto = new AssignmentUpdateDTO
            {
                Id = 1,
                CourseId = 1,
                TopicId = 1,
                AssignmentName = "Test Assignment",
                AssignmentDescription = "Test Assignment Description",
                MaxMark = 10,
                MinMark = 5,
                IsRequired = true,
                AssignmentDeadline = DateTime.UtcNow.AddDays(7),
            };
            var assignmentFormData = new MultipartFormDataContent
             {
                 { new StringContent(assignmentDto.Id.ToString()), "Id" },
                 { new StringContent(assignmentDto.CourseId.ToString()), "CourseId" },
                 { new StringContent(assignmentDto.TopicId.ToString()!), "TopicId" },
                 { new StringContent(assignmentDto.AssignmentName), "AssignmentName" },
                 { new StringContent(assignmentDto.AssignmentDescription), "AssignmentDescription" },
                 { new StringContent(assignmentDto.MaxMark.ToString()), "MaxMark" },
                 { new StringContent(assignmentDto.MinMark.ToString()), "MinMark" },
                 { new StringContent(assignmentDto.IsRequired.ToString()), "IsRequired" },
                 { new StringContent(assignmentDto.AssignmentDeadline.ToString("o")), "AssignmentDeadline" },
             };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.PutAsync($"{baseUrl}/update", assignmentFormData);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }
            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var updatedAssignment = jsonObject?.TryGetValue("result", out var resultToken) == true
                       ? resultToken.ToObject<AssignmentOutDTO>() : null;

            _testOutputHelper.WriteLine
                (
                    $"Id:\t{updatedAssignment!.Id}\n" +
                    $"Name:\t{updatedAssignment.AssignmentName}\n" +
                    $"Edited time:\t{updatedAssignment.EditedTime}\n" +
                    $"Is Edited:\t{updatedAssignment.IsEdited}\n"
                );
        }

        [Fact]
        public async Task GetByIdAssignment_ValidId_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.GetAsync($"{baseUrl}/getById/{1}");

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }
            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var actualAssignment = jsonObject?.TryGetValue("result", out var resultToken) == true
                       ? resultToken.ToObject<AssignmentOutDTO>() : null;

            Assert.NotNull(actualAssignment);
            Assert.Equal(1, actualAssignment.Id);
            _testOutputHelper.WriteLine(
                "Id:\t" + $"{actualAssignment.Id}" +
                "\nName:\t" + $"{actualAssignment.AssignmentName}" +
                "\nDescription:\t" + $"{actualAssignment.AssignmentDescription}");
        }

        [Fact]
        public async Task DeleteAssignment_ValidAssignment_ReturnsOk()
        {
            // Arrange
            int id = 2;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.DeleteAsync($"{baseUrl}/delete/{id}");

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            if (jsonObject is null)
            {
                Assert.Fail("Response content is empty");
            }
            var resultValue = jsonObject["result"]?.ToString();

            Assert.Equal("Deleted was successful", resultValue);
            _testOutputHelper.WriteLine($"{resultValue}");
        }

        [Fact]
        public async Task RemoveAssignments_ValidEntities_ReturnsOk()
        {
            // Arrange
            var entities = new List<int> { 3, 4 };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var jsonContent = JsonConvert.SerializeObject(entities);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{baseUrl}/removeList"),
                Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            if (jsonObject is null)
            {
                Assert.Fail("Response content is empty");
            }
            var resultValue = jsonObject["result"]?.ToString();

            Assert.Equal("Deleted was successful", resultValue);
            _testOutputHelper.WriteLine($"{resultValue}");
        }

        [Fact]
        public async Task AddAssignmentLink_ValidLinkAndId_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);
            var link = "https://example.com/link";
            var id = 1;
            var jsonContent = JsonConvert.SerializeObject(link);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"{baseUrl}/addLink/{id}", content);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            if (jsonObject is null)
            {
                Assert.Fail("Response content is empty");
            }

            var addedLink = jsonObject?.TryGetValue("result", out var resultToken) == true
                       ? resultToken.ToObject<AssignmentlinkOutDTO>() : null;

            Assert.Equal(link, addedLink?.AssignmentLink);
            _testOutputHelper.WriteLine(
                "Id:\t" + $"{addedLink?.Id}" +
                "\nName:\t" + $"{addedLink?.AssignmentLink}");
        }

        [Fact]
        public async Task DeleteAssignmentLinkById_ValidLinkId_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            var linkId = 1;
            // Act
            var response = await _client.DeleteAsync($"{baseUrl}/deleteLinkById/{linkId}");

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            if (jsonObject is null)
            {
                Assert.Fail("Response content is empty");
            }
            var resultValue = jsonObject["result"]?.ToString();

            Assert.Equal("Deleted was successful", resultValue);
            _testOutputHelper.WriteLine($"{resultValue}");
        }
       
    }
}
