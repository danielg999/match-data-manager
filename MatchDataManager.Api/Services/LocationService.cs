using FluentValidation;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Models.Validations;
using MatchDataManager.Api.Repository;

namespace MatchDataManager.Api.Services;

public class LocationService : ILocationService
{
    private readonly IValidator<LocationCreateDto> _locationCreateDtoValidator;
    private readonly IValidator<LocationUpdateDto> _locationUpdateDtoValidator;
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository, IValidator<LocationCreateDto> locationCreateDtoValidator, IValidator<LocationUpdateDto> locationUpdateDtoValidator)
    {
        _locationRepository = locationRepository;
        _locationCreateDtoValidator = locationCreateDtoValidator;
        _locationUpdateDtoValidator = locationUpdateDtoValidator;
    }

    public async Task<Guid> Create(LocationCreateDto dto)
    {
        await CheckCreateValidation(dto);
        var locationId = await _locationRepository.Create(dto);

        return locationId;
    }

    public async Task Delete(Guid id)
    {
        await _locationRepository.Delete(id);
    }

    public async Task<IEnumerable<LocationDto>> GetAll()
    {
        return await _locationRepository.GetAll();
    }

    public async Task<LocationDto> Get(Guid id)
    {
        return await _locationRepository.Get(id);
    }

    public async Task Update(LocationUpdateDto dto)
    {
        await CheckUpdateValidation(dto);
        await _locationRepository.Update(dto);
    }

    public async Task CheckCreateValidation(LocationCreateDto dto)
    {
        var validations = _locationCreateDtoValidator.Validate(dto);

        if (!validations.IsValid)
        {
            throw new ValidationException(validations.Errors.ToString());
        }
    }

    public async Task CheckUpdateValidation(LocationUpdateDto dto)
    {
        var validations = _locationUpdateDtoValidator.Validate(dto);

        if (!validations.IsValid)
        {
            throw new ValidationException(validations.Errors.ToString());
        }
    }

    public async Task<int> CountAllLocations()
    {
        return await _locationRepository.CountAllLocations();
    }
}