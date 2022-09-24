using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Repository
{
    public interface ILocationRepository
    {
        Task<Guid> Create(LocationCreateDto dto);

        Task<bool> IsAnyExistOnCreate(string value);
        Task<bool> IsAnyExistOnUpdate(string name, Guid id);
        Task Delete(Guid id);
        Task<IEnumerable<LocationDto>> GetAll();
        Task<LocationDto> Get(Guid id);
        Task Update(LocationUpdateDto dto);
        Task<int> CountAllLocations();
    }
}
