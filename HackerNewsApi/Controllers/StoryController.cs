namespace HackerNews.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using HackerNews.Data.Models;
    using HackerNews.Business.Services;
    using Microsoft.AspNetCore.Mvc;
    using HackerNews.Business.Interfaces;

    /// <summary>
    /// Defines the <see cref="StoryController" />.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class StoryController : ControllerBase
    {
        /// <summary>
        /// Defines the _storyService.
        /// </summary>
        private readonly IStoryService _storyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryController"/> class.
        /// </summary>
        /// <param name="storyService">The service<see cref="IStoryService"/>.</param>

        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        /// <summary>
        /// Get top stories details.
        /// </summary>
        /// <returns>
        /// <see cref="ServiceResponse{}"/> representing the result of the operation.
        /// </returns>

        [HttpGet]
        [Route("getTopStories")]
        public async Task<ActionResult<IEnumerable<Story>>> GetTopStories()
        {
            try
            {
                IEnumerable<Story> result = await _storyService.GetTopStoriesAsync();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HttpStatusCode.BadRequest);
            }
        }
    }
}
 
