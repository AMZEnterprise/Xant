using System.Threading.Tasks;
using Xant.Core.Domain;

namespace Xant.Core.Repositories
{
    /// <summary>
    /// Represents setting repository interface 
    /// </summary>
    public interface ISettingRepository
    {
        /// <summary>
        /// Get setting
        /// </summary>
        /// <returns>returns setting</returns>
        Task<Setting> Get();
        /// <summary>
        /// Update setting
        /// </summary>
        /// <param name="setting">updated setting</param>
        void Update(Setting setting);
    }
}
