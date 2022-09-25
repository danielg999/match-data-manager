using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Services;

public interface ITeamService
{
    Task<Guid> Create(TeamCreateDto team);

    Task Delete(Guid id);

    Task<IEnumerable<TeamDto>> GetAll();

    Task<TeamDto> Get(Guid id);

    Task Update(TeamUpdateDto team);
}