using AutoMapper;
using FluentValidation;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Repository;
using Microsoft.EntityFrameworkCore;

namespace MatchDataManager.Api.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IValidator<TeamCreateDto> _teamCreateDtoValidator;
    private readonly IValidator<TeamUpdateDto> _teamUpdateDtoValidator;

    public TeamService(ITeamRepository teamRepository, IValidator<TeamCreateDto> teamCreateDtoValidator, IValidator<TeamUpdateDto> teamUpdateDtoValidator)
    {
        _teamRepository = teamRepository;
        _teamCreateDtoValidator = teamCreateDtoValidator;
        _teamUpdateDtoValidator = teamUpdateDtoValidator;
    }

    public async Task<Guid> Create(TeamCreateDto dto)
    {
        await CheckCreateValidation(dto);
        return await _teamRepository.Create(dto);

    }

    public async Task Delete(Guid id)
    {
        await _teamRepository.Delete(id);
    }

    public async Task<IEnumerable<TeamDto>> GetAll()
    {
        return await _teamRepository.GetAll();
    }

    public async Task<TeamDto> Get(Guid id)
    {
        return await _teamRepository.Get(id);
    }

    public async Task Update(TeamUpdateDto dto)
    {
        await CheckUpdateValidation(dto);
        await _teamRepository.Update(dto);
    }

    public async Task CheckCreateValidation(TeamCreateDto dto)
    {
        var validations = _teamCreateDtoValidator.Validate(dto);

        if (!validations.IsValid)
        {
            throw new BadRequestException(validations.Errors.ToString());
        }
    }

    public async Task CheckUpdateValidation(TeamUpdateDto dto)
    {
        var validations = _teamUpdateDtoValidator.Validate(dto);

        if (!validations.IsValid)
        {
            throw new BadRequestException(validations.Errors.ToString());
        }
    }

    public async Task<int> CountAllTeams()
    {
        return await _teamRepository.CountAllTeams();
    }
}