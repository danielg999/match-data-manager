using FluentValidation;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Repository;

namespace MatchDataManager.Api.Models.Validations
{
    public class LocationUpdateDtoValidator : AbstractValidator<LocationUpdateDto>
    {
        private readonly ILocationRepository _locationRepository;

        public LocationUpdateDtoValidator(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;

            RuleFor(l => l.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(l => new { l.Name, l.Id })
                .Custom(async (value, context) =>
                {
                    var nameInUse = await _locationRepository.IsAnyExistOnUpdate(value.Name, value.Id);
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