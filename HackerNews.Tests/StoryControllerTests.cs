using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using HackerNews.API.Controllers;
using HackerNews.Business.Services;
using HackerNews.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HackerNews.Business.Interfaces;

namespace HackerNewsApi.Test
{
    [TestFixture]
    public class StoryControllerTests
    {
        private StoryController _controller;
        private Mock<IStoryService> _mockNewsService;
        private List<Story> _lstStories;

        [SetUp]
        public void Setup()
        {
            // Initialize Mock Service
            _mockNewsService = new Mock<IStoryService>();

            // Inject Mock Service into Controller
            _controller = new StoryController(_mockNewsService.Object);

            /* mock data setup */
            _lstStories = new List<Story>(){
            new Story()
            {
                User = "dhouston",
                By="test1",
                Url = "http://www.getdropbox.com/u/2/screencast.html",
                Title = "My YC app: Dropbox - Throw away your USB drive story1",
                Type="story"
            },
            new Story()
            {
                By="Rediff",
                Title="Rediff News",
                Url="www.Rediff.com",
                User = "dhoon",
                Type="story"
            }
        };
        }

        [Test]
        public async Task GetStoryList_Check_Not_Null()
        {
            // Arrange: Set up any necessary data or mocks
            _mockNewsService.Setup(p => p.GetTopStoriesAsync()).ReturnsAsync(_lstStories);

            // Act: Call the controller action
            var result = await _controller.GetTopStories();
            var expectedResult = result.Result;

            // Assert: Verify the result
            Assert.IsNotNull(expectedResult);
        }

        [Test]
        public async Task GetStoryList_ReturnAllStories_Result_Ok()
        {
            // Arrange: Set up any necessary data or mocks
            _mockNewsService.Setup(p => p.GetTopStoriesAsync()).ReturnsAsync(_lstStories);

            // Act: Call the controller action
            var result =  await _controller.GetTopStories();

            // Assert: Verify the result
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);    
        }

        [Test]
        public async Task GetStoryList_Returns_ListOfItems()
        {
            // Arrange: Set up any necessary data or mocks
            _mockNewsService.Setup(p => p.GetTopStoriesAsync()).ReturnsAsync(_lstStories);


            // Act: Call the controller action
            var result = await _controller.GetTopStories();

            // Assert: Verify the result
            var okResult = (OkObjectResult)result.Result;
            var items = (List<Story>)okResult.Value;
            Assert.AreEqual(_lstStories.Count,items.Count);
            CollectionAssert.AreEqual(_lstStories, items);
        }
    }
}