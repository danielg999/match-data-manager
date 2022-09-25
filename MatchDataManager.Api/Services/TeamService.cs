using AutoMapper;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Guid> Create(TeamCreateDto dto)
    {
        var team = _mapper.Map<Team>(dto);

        team.Id = Guid.NewGuid();

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync();

        return team.Id;
    }

    public async Task Delete(Guid teamId)
    {
        var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Id == teamId);

        if (team is null)
        {
            throw new NotFoundException("Team not found.");
        }

        _dbContext.Teams.Remove(team);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TeamDto>> GetAll()
    {
        var teams = await _dbContext.Teams.ToListAsync();
        var teamsDto = _mapper.Map<List<TeamDto>>(teams);

        return teamsDto;
    }

    public async Task<TeamDto> Get(Guid id)
    {
        var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Id == id);
        var teamDto = _mapper.Map<TeamDto>(team);

        return teamDto;
    }

    public async Task Update(TeamUpdateDto dto)
    {
        var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Id == dto.Id);

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

        await _dbContext.SaveChangesAsync();
    }
}