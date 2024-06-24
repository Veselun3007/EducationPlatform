using CourseContent.Core.DTO.Requests;
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
    public class MaterialControllerTests(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        : BaseIntegrationTest(factory)
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;
        const int courseId = 1;

        [Fact]
        public async Task CreateMaterial_ValidMaterial_ReturnsOk()
        {
            // Arrange
            var materialDto = new MaterialDTO
            {
                CourseId = 1,
                TopicId = 1,
                MaterialName = "New material 1",
                MaterialDescription = "Description of new material 1",
                MaterialDatePublication = DateTime.UtcNow,
                MaterialFiles = [],
                MaterialLinks = ["https://example.com/link1", "https://example.com/link2"]
            };

            var materialFormData = new MultipartFormDataContent
            {
                { new StringContent(materialDto.CourseId.ToString()), "CourseId" },
                { new StringContent(materialDto.TopicId.ToString()!), "TopicId" },
                { new StringContent(materialDto.MaterialName), "MaterialName" },
                { new StringContent(materialDto.MaterialDescription), "MaterialDescription" },
                { new StringContent(materialDto.MaterialDatePublication.ToString("o")), "MaterialDatePublication" },
            };
            foreach (var link in materialDto.MaterialLinks)
            {
                materialFormData.Add(new StringContent(link), $"MaterialLinks[{materialDto.MaterialLinks.IndexOf(link)}]");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.PostAsync($"{Setup.MaterialBaseURL}/create", materialFormData);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var createdMaterial = jsonObject?.TryGetValue("result", out var resultToken) == true
                               ? resultToken.ToObject<MaterialOutDTO>() : null;

            Assert.NotNull(createdMaterial);
            _testOutputHelper.WriteLine
                (
                    $"Material Id:\t{createdMaterial.Id}\n" +
                    $"Material Name:\t{createdMaterial.MaterialName}\n" +
                    $"Material Description:\t{createdMaterial.MaterialDescription}\n" +
                    $"Material Date Publication:\t{createdMaterial.MaterialDatePublication}\n"
                );
        }

        [Fact]
        public async Task GetAllMaterials_ReturnsExpectedMaterials()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.GetAsync($"{Setup.MaterialBaseURL}/getAll/{courseId}");

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var materialsJson = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(materialsJson))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonArray = JsonConvert.DeserializeObject<JArray>(materialsJson);
            if (jsonArray is null)
            {
                Assert.Fail("Response content is empty");
            }

            var materials = jsonArray.ToObject<List<MaterialOutDTO>>();

            Assert.NotNull(materials);
            Assert.NotEmpty(materials);
            _testOutputHelper.WriteLine($"Row count:\t {materials.Count}");
        }

        [Fact]
        public async Task UpdateMaterial_ValidMaterial_ReturnsOk()
        {
            // Arrange
            var materialUpdateDto = new MaterialUpdateDTO
            {
                Id = 1,
                CourseId = 1,
                TopicId = 1,
                MaterialName = "Updated material 1",
                MaterialDescription = "Updated description of material 1",
                MaterialDatePublication = DateTime.UtcNow
            };
            var materialFormData = new MultipartFormDataContent
            {
                { new StringContent(materialUpdateDto.Id.ToString()), "Id" },
                { new StringContent(materialUpdateDto.CourseId.ToString()), "CourseId" },
                { new StringContent(materialUpdateDto.TopicId.ToString()!), "TopicId" },
                { new StringContent(materialUpdateDto.MaterialName), "MaterialName" },
                { new StringContent(materialUpdateDto.MaterialDescription), "MaterialDescription" },
                { new StringContent(materialUpdateDto.MaterialDatePublication.ToString("o")), "MaterialDatePublication" },
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.PutAsync($"{Setup.MaterialBaseURL}/update", materialFormData);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }
            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var updatedMaterial = jsonObject?.TryGetValue("result", out var resultToken) == true
                               ? resultToken.ToObject<MaterialOutDTO>() : null;

            _testOutputHelper.WriteLine
                (
                    $"Material Id:\t{updatedMaterial!.Id}\n" +
                    $"Material Name:\t{updatedMaterial.MaterialName}\n" +
                    $"Material Edited time:\t{updatedMaterial.EditedTime}\n" +
                    $"Material Is Edited:\t{updatedMaterial.IsEdited}\n"
                );
        }

        [Fact]
        public async Task GetMaterialById_ValidId_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.GetAsync($"{Setup.MaterialBaseURL}/getById/{1}");

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }
            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var actualMaterial = jsonObject?.TryGetValue("result", out var resultToken) == true
                              ? resultToken.ToObject<MaterialOutDTO>() : null;

            Assert.NotNull(actualMaterial);
            Assert.Equal(1, actualMaterial.Id);
            _testOutputHelper.WriteLine(
                "Id:\t" + $"{actualMaterial.Id}" +
                "\nName:\t" + $"{actualMaterial.MaterialName}" +
                "\nDescription:\t" + $"{actualMaterial.MaterialDescription}");
        }

        [Fact]
        public async Task DeleteMaterial_ValidMaterial_ReturnsOk()
        {
            // Arrange
            int id = 2;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var response = await _client.DeleteAsync($"{Setup.MaterialBaseURL}/delete/{id}");

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
        public async Task RemoveMaterials_ValidEntities_ReturnsOk()
        {
            // Arrange
            var entities = new List<int> { 3, 4 };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            // Act
            var jsonContent = JsonConvert.SerializeObject(entities);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{Setup.MaterialBaseURL}/removeList"),
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
        public async Task AddMaterialLink_ValidLinkAndId_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);
            var id = 1;
            var link = "https://materialeExample.com/link";
            var jsonContent = JsonConvert.SerializeObject(link);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"{Setup.MaterialBaseURL}/addLink/{id}", content);

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
                       ? resultToken.ToObject<MateriallinkOutDTO>() : null;

            Assert.Equal(link, addedLink?.MaterialLink);
            _testOutputHelper.WriteLine(
                "Id:\t" + $"{addedLink?.Id}" +
                "\nName:\t" + $"{addedLink?.MaterialLink}");
        }

        [Fact]
        public async Task DeleteMaterialLinkById_ValidLinkId_ReturnsOk()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.Token);

            var linkId = 1;
            // Act
            var response = await _client.DeleteAsync($"{Setup.MaterialBaseURL}/deleteLinkById/{linkId}");

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
