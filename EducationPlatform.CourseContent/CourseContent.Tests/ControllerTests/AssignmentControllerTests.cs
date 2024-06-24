using CourseContent.Core.DTO.Requests.AssignmentDTO;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Tests.Base;
using CourseContent.Tests.Config;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit.Abstractions;

namespace CourseContent.Tests.ControllerTests
{
    public class AssignmentControllerTests(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        : BaseIntegrationTest(factory)
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;
        const int courseId = 1;

        [Fact]
        public async Task CreateAssignment_ReturnsOk()
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
                AssignmentLinks = ["https://example.com/link1", "https://example.com/link2" ],
            };

            var projectDir = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
            var filePath = Path.Combine(projectDir, "Assets", "imgForTest.jpg");
            var fileStream = File.OpenRead(filePath);
            var fileStreamContent = new StreamContent(fileStream);
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);
            
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
                assignmentFormData.Add(new StringContent(link), "AssignmentLinks");
            }

            assignmentFormData.Add(fileStreamContent, "AssignmentFiles", "imgForTest.jpg");

            // Act
            var response = await _client.PostAsync($"{Setup.AssignmentBaseURL}/create", assignmentFormData);

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
                    $"Is Edited:\t{createdAssignment.IsEdited}\n" +
                    $"Link:\t{createdAssignment.Assignmentlinks?.Count}\n" +
                    $"File:\t{createdAssignment.Assignmentfiles?.Count}\n"
                );
        }

        [Fact]
        public async Task GetAllAssignment_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.GetAsync($"{Setup.AssignmentBaseURL}/getAll/{courseId}");

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
            foreach (AssignmentOutDTO assignment in assignments)
            {
                _testOutputHelper.WriteLine(assignment.AssignmentName);
            }
        }

        [Fact]
        public async Task UpdateAssignment_ReturnsOk()
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

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.PutAsync($"{Setup.AssignmentBaseURL}/update", assignmentFormData);

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
        public async Task GetByIdAssignment_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.GetAsync($"{Setup.AssignmentBaseURL}/getById/{1}");

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
        public async Task DeleteAssignment_ReturnsOk()
        {
            // Arrange
            int id = 2;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.DeleteAsync($"{Setup.AssignmentBaseURL}/delete/{id}");

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
        public async Task RemoveAssignments_ReturnsOk()
        {
            // Arrange
            var entities = new List<int> { 3, 4 };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var jsonContent = JsonConvert.SerializeObject(entities);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{Setup.AssignmentBaseURL}/removeList"),
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
        public async Task AddAssignmentLink_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);
            var link = "https://example.com/link";
            var id = 1;
            var jsonContent = JsonConvert.SerializeObject(link);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"{Setup.AssignmentBaseURL}/addLink/{id}", content);

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
        public async Task DeleteAssignmentLinkById_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            var linkId = 1;
            // Act
            var response = await _client.DeleteAsync($"{Setup.AssignmentBaseURL}/deleteLinkById/{linkId}");

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
        public async Task AddAssignmentFile_ReturnsOk()
        {
            // Arrange
            byte[] bytes;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file content")),
                0, Encoding.UTF8.GetBytes("Test file content").Length, "data.txt", "testFile.txt");
            var id = 1;

            // Act
            using (var stream = file.OpenReadStream())
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            var request = new MultipartFormDataContent
            {
                { new ByteArrayContent(bytes), "file", file.Name }
            };

            var response = await _client.PostAsync($"{Setup.AssignmentBaseURL}/addFile/{id}", request);

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

            var addedFile = jsonObject?.TryGetValue("result", out var resultToken) == true
                       ? resultToken.ToObject<AssignmentfileOutDTO>() : null;
            _testOutputHelper.WriteLine(
                        $"Id:\t{addedFile?.Id}" +
                        $"\nFileLink:\t {addedFile?.AssignmentFile}");
        }

        [Fact]
        public async Task GetByIdAssignmentFile_ReturnsOk()
        {
            // Arrange
            var id = 1;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.GetAsync($"{Setup.AssignmentBaseURL}/getFileById/{id}");

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

            var fileLink = jsonObject?.TryGetValue("result", out var resultToken) == true
                      ? resultToken.ToString() : null;

            var receivedFile = new AssignmentfileOutDTO { AssignmentFile = fileLink };

            _testOutputHelper.WriteLine($"\nFileLink:\t {receivedFile?.AssignmentFile}");
        }

        [Fact]
        public async Task DeleteByIdAssignmentFile_ReturnsOk()
        {
            // Arrange
            var id = 2;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.DeleteAsync($"{Setup.AssignmentBaseURL}/deleteFileById/{id}");

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
