using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Repository
{
    public interface ITeamRepository
    {
        Task<Guid> Create(TeamCreateDto dto);

        Task<bool> IsAnyExistOnCreate(string value);
        Task<bool> IsAnyExistOnUpdate(string name, Guid id);
        Task Delete(Guid id);
        Task<IEnumerable<TeamDto>> GetAll();
        Task<TeamDto> Get(Guid id);
        Task Update(TeamUpdateDto dto);
        Task<int> CountAllTeams();
    }
}
