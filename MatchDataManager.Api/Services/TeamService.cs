using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Services;

public class TeamsService : ITeamsService
{
    private readonly List<Team> _teams = new();

    public void AddTeam(Team team)
    {
        team.Id = Guid.NewGuid();
        _teams.Add(team);
    }

    public void DeleteTeam(Guid teamId)
    {
        var team = _teams.FirstOrDefault(x => x.Id == teamId);
        if (team is not null)
        {
            _teams.Remove(team);
        }
    }

    public IEnumerable<Team> GetAllTeams()
    {
        return _teams;
    }

    public Team GetTeamById(Guid id)
    {
        return _teams.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateTeam(Team team)
    {
        var existingTeam = _teams.FirstOrDefault(x => x.Id == team.Id);
        if (existingTeam is null || team is null)
        {
            throw new ArgumentException("Team doesn't exist.", nameof(team));
        }

        existingTeam.CoachName = team.CoachName;
        existingTeam.Name = team.Name;
    }
}