namespace HackerNews.Business.Services
{
    using HackerNews.Data.Models;
    using Newtonsoft.Json;
    using HackerNews.Data.Constants;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using HackerNews.Business.Interfaces;

    /// <summary>
    /// Defines the <see cref="StoryService" />.
    /// </summary>
    public class StoryService : IStoryService
    {
        /// <summary>
        /// Defines the _memoryCache.
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        private static HttpClient _client = new HttpClient();
        public StoryService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// method to get top 200 story details.
        /// </summary>
        /// <returns>
        /// IEnumerable<Story>
        /// </returns>
        public async Task<IEnumerable<Story>> GetTopStoriesAsync()
        {
            List<Story> lstStory = new List<Story>();

            // Try to retrieve data from cache if exists
            if (!_memoryCache.TryGetValue($"{ApiConfigKeys.CacheKey}", out List<Story> result))
            {
                string url = string.Concat(ApiConfigKeys.ApiBaseUrl, "topstories.json?print=pretty");

                HttpResponseMessage response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var storyResponse = response.Content.ReadAsStringAsync().Result;
                    var topStoryIds = JsonConvert.DeserializeObject<List<int>>(storyResponse);
                    if (topStoryIds != null)
                    {
                        var batchSize = 50;

                        /* to get only top 200 stories and batchSize is used to handle multiple api calls in not single thread. */
                        int totalStoryIds = (int)Math.Ceiling((double)topStoryIds.Take(200).Count() / batchSize);

                        for (int i = 0; i < totalStoryIds; i++)
                        {
                            var currentIds = topStoryIds.Skip(i * batchSize).Take(batchSize);
                            var tasks = currentIds.Select(id => GetStoryByIdAsync(id));
                            lstStory.AddRange(await Task.WhenAll(tasks));
                        }
                    }

                    // Save data in memory cache
                    _memoryCache.Set($"{ApiConfigKeys.CacheKey}", lstStory, TimeSpan.FromMinutes(10));
                }
            }
            else {
                lstStory = new List<Story>(result);
            }
            return await Task.FromResult<IEnumerable<Story>>(lstStory);
        }

        /// <summary>
        /// To fetch story from firebase api by story Id.
        /// </summary>
        /// <param name="storyId">storyId.</param>
        /// <returns>
        /// <Story>
        /// </returns>
        public async Task<Story> GetStoryByIdAsync(int storyId)
        {
            var url = string.Concat(string.Format("{0}{1}", ApiConfigKeys.ApiBaseUrl, string.Format("item/{0}.json", storyId)));
            HttpResponseMessage response = await _client.GetAsync(url);
            var storyResponse = response.Content.ReadAsStringAsync().Result;
            Story story = JsonConvert.DeserializeObject<Story>(storyResponse);
            return story;
        }

        // To featch In memory caching data.
        public List<Story> GetInMemoryCacheList(string key)
        {
                return _memoryCache.Get<List<Story>>(key);
        }

    }

}
