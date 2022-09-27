using AutoMapper;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MatchDataManager.Api.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IMapper _mapper;
        private readonly MatchDataManagerDbContext _dbContext;

        public TeamRepository(IMapper mapper, MatchDataManagerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Guid> Create(TeamCreateDto dto)
        {
            var team = _mapper.Map<Team>(dto);

            team.Id = Guid.NewGuid();
            _dbContext.Teams.Add(team);
            await _dbContext.SaveChangesAsync();
            return team.Id;
        }

        public async Task<bool> IsAnyExistOnCreate(string name)
        {
            return await _dbContext.Teams.AnyAsync(l => l.Name == name);
        }

        public async Task<bool> IsAnyExistOnUpdate(string name, Guid id)
        {
            return await _dbContext.Teams.AnyAsync(l => l.Name == name && l.Id != id);
        }

        public async Task Delete(Guid id)
        {
            var team = _dbContext.Teams.FirstOrDefault(x => x.Id == id);

            if (team is null)
            {
                throw new NotFoundException("Team not found.");
            }

            _dbContext.Teams.Remove(team);
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<TeamDto>> GetAll()
        {
            var teams = await _dbContext.Teams.ToListAsync();
            var teamsDtos = _mapper.Map<List<TeamDto>>(teams);

            return teamsDtos;
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
            await Task.CompletedTask;
        }

        public async Task<int> CountAllTeams()
        {
            return await _dbContext.Teams.CountAsync();
        }
    }
}