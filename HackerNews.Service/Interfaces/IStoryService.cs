using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.Data.Models;

namespace HackerNews.Business.Interfaces
{

    /// <summary>
    /// Defines the <see cref="IStoryService" />.
    /// </summary>
    public interface IStoryService
    {
        /// <summary>
        /// Get top story details.
        /// </summary>
        /// <returns>List<Story></returns>
        Task<IEnumerable<Story>> GetTopStoriesAsync();

        Task<Story> GetStoryByIdAsync(int id);


    }
}
