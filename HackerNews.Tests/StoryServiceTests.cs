using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using HackerNews.Business.Services;
using System.Threading.Tasks;
using HackerNews.Data.Models;

namespace hackernewsapi.Tests
{
    [TestFixture]
    public class StoryServiceTests
    {
        private StoryService _storyservice;
        private IMemoryCache _memoryCache;
        private List<Story> _lstStories;

        [SetUp]
        public void Setup()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _storyservice = new StoryService(_memoryCache);

            /* mock data to test */
            _lstStories = new List<Story>() {
                new Story()
                {
                    Title = "My YC app: Dropbox - Throw away your USB drive story1",
                    User = "dhouston", By="test1",
                    Url = "http://www.getdropbox.com/u/1/screencast.html"
                },
                new Story()
                {
                    Title = "My YC app: Gmail - Throw away",
                    User = "dhouston", By="test1",
                    Url = "http://www.google.com/u/2/screencast.html"
                }
            };
        }

        [Test]
        public async Task GetStoryList_Returns_From_Cache_If_Available()
        {
            // Arrange
            string cacheKey = "story_key";

            // Act: Call the service method
            var result = _storyservice.GetTopStoriesAsync().Result;
            var cachedList = _storyservice.GetInMemoryCacheList(cacheKey);

            // Assert: Verify the result
            Assert.IsNotNull(cachedList);
            Assert.IsNotNull(result);
            var lstStories = result as List<Story>;
            Assert.AreEqual(lstStories.Count, cachedList.Count);
            CollectionAssert.AreEqual(lstStories, cachedList);

        }

        [Test]
        public async Task GetStoryList_Not_Returns_From_Cache_If_Not_Available()
        {
            // Arrange
            string cacheKey = "Non_Existing_key";

            // Act: Call the service method
            var result = _storyservice.GetTopStoriesAsync().Result;
            var cachedList = _storyservice.GetInMemoryCacheList(cacheKey);

            // Assert: Verify the result
            Assert.IsNotNull(result);
            Assert.IsNull(cachedList);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryCache.Dispose();
        }

    }
}