using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Services;

public interface ITeamService
{
    void AddTeam(Team team);
    void DeleteTeam(Guid teamId);
    IEnumerable<Team> GetAllTeams();
    Team GetTeamById(Guid id);
    void UpdateTeam(Team team);
}