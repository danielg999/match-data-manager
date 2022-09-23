using AutoMapper;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Api.Services;

public class TeamService : ITeamService
{
    private readonly MatchDataManagerDbContext _dbContext;
    private readonly IMapper _mapper;

    public TeamService(MatchDataManagerDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Guid Create(TeamCreateDto dto)
    {
        var team = _mapper.Map<Team>(dto);

        team.Id = Guid.NewGuid();

        _dbContext.Teams.Add(team);
        _dbContext.SaveChanges();

        return team.Id;
    }

    public void Delete(Guid teamId)
    {
        var team = _dbContext.Teams.FirstOrDefault(x => x.Id == teamId);

        if (team is null)
        {
            throw new NotFoundException("Team not found.");
        }

        _dbContext.Teams.Remove(team);
        _dbContext.SaveChanges();
    }

    public IEnumerable<TeamDto> GetAll()
    {
        var teams = _dbContext.Teams.ToList();
        var teamsDto = _mapper.Map<List<TeamDto>>(teams);

        return teamsDto;
    }

    public TeamDto Get(Guid id)
    {
        var team = _dbContext.Teams.FirstOrDefault(x => x.Id == id);
        var teamDto = _mapper.Map<TeamDto>(team);

        return teamDto;
    }

    public void Update(Guid id, TeamUpdateDto dto)
    {
        var team = _dbContext.Teams.FirstOrDefault(x => x.Id == id);

        if (dto is null)
        {
            throw new BadRequestException("Team doesn't exist.");
        }
        else if (team is null)
        {
            throw new NotFoundException("Team not found.");
        }

        team.CoachName = dto.CoachName;
        team.Name = dto.Name;

        _dbContext.SaveChanges();
    }
}