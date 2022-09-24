using FluentValidation;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Repository;

namespace MatchDataManager.Api.Models.Validations
{
    public class LocationCreateDtoValidator : AbstractValidator<LocationCreateDto>
    {
        private readonly ILocationRepository _locationRepository;

        public LocationCreateDtoValidator(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;

            RuleFor(l => l.Name)
                .NotEmpty()
                .MaximumLength(255)
                .Custom(async (value, context) =>
                {
                    var nameInUse = await _locationRepository.IsAnyExistOnCreate(value);
                    if (nameInUse)
                    {
                        context.AddFailure("Name", "That name is taken.");
                    }
                });

            RuleFor(l => l.City)
                .NotEmpty()
                .MaximumLength(55);
        }
    }
}