using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Services;

public interface ITeamService
{
    Guid Create(TeamCreateDto team);

    void Delete(Guid id);

    IEnumerable<TeamDto> GetAll();

    TeamDto Get(Guid id);

    void Update(TeamUpdateDto team);
}