﻿using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Tests.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xunit.Abstractions;
using System.Text;
using CourseContent.Tests.Config;

namespace CourseContent.Tests.ControllerTests
{
    public class TopicControllerTest(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        : BaseIntegrationTest(factory)
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;
        private const string baseUrl = "https://localhost:5002/api/topic";
        const int courseId = 1;

        [Fact]
        public async Task CreateTopic_ValidTopic_ReturnsOk()
        {
            // Arrange
            var topicDto = new TopicDTO
            {
                CourseId = courseId,
                Title = "New topic 4"
            };

            var topicFormData = new MultipartFormDataContent
            {
                { new StringContent(topicDto.CourseId.ToString()), "CourseId" },
                { new StringContent(topicDto.Title), "Title" }
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.PostAsync($"{baseUrl}/create", topicFormData);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var createdTopic = jsonObject?.TryGetValue("result", out var resultToken) == true
                          ? resultToken.ToObject<TopicOutDTO>() : null;

            Assert.NotNull(createdTopic);
            _testOutputHelper.WriteLine
                (
                    $"Topic Id:\t{createdTopic.Id}\n" +
                    $"Topic Title:\t{createdTopic.Title}\n"
            );
        }

        [Fact]
        public async Task GetAllTopics_ReturnsExpectedTopics()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.GetAsync($"{baseUrl}/getAll/{courseId}");

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);

            var topicsJson = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(topicsJson))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonArray = JsonConvert.DeserializeObject<JArray>(topicsJson);
            if (jsonArray is null)
            {
                Assert.Fail("Response content is empty");
            }

            var topics = jsonArray.ToObject<List<TopicOutDTO>>();

            Assert.NotNull(topics);
            Assert.NotEmpty(topics);
            _testOutputHelper.WriteLine($"Row count:\t {topics.Count}");
        }

        [Fact]
        public async Task UpdateTopic_ValidTopic_ReturnsOk()
        {
            // Arrange
            var topicUpdateDto = new TopicUpdateDTO
            {
                Id = 1,
                CourseId = 1,
                Title = "Updated topic 1"
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(topicUpdateDto), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Setup.token);

            // Act
            var response = await _client.PutAsync($"{baseUrl}/update", jsonContent);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("Response content is empty");
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
            var updatedTopic = jsonObject?.TryGetValue("result", out var resultToken) == true
                         ? resultToken.ToObject<TopicOutDTO>() : null;

            _testOutputHelper.WriteLine
                (
                    $"Topic Id:\t{updatedTopic!.Id}\n" +
                    $"Topic Title:\t{updatedTopic.Title}\n"
                );
        }

        [Fact]
        public async Task GetTopicById_ValidId_ReturnsOk()
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
            var actualTopic = jsonObject?.TryGetValue("result", out var resultToken) == true
                          ? resultToken.ToObject<TopicOutDTO>() : null;

            Assert.NotNull(actualTopic);
            Assert.Equal(1, actualTopic.Id);
            _testOutputHelper.WriteLine(
                "Id:\t" + $"{actualTopic.Id}" +
                "\nTopic name:\t" + $"{actualTopic.Title}"
            );
        }

        [Fact]
        public async Task DeleteTopic_ValidTopic_ReturnsOk()
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
    }
}
