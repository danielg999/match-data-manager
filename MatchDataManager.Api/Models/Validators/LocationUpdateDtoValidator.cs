using FluentValidation;
using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Models.Validations
{
    public class LocationUpdateDtoValidator : AbstractValidator<LocationUpdateDto>
    {
        private readonly MatchDataManagerDbContext _dbContext;

        public LocationUpdateDtoValidator(MatchDataManagerDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(l => l.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(l => new { l.Name, l.Id })
                .Custom((value, context) =>
                {
                    var nameInUse = _dbContext.Locations.Any(l => l.Name == value.Name && l.Id != value.Id);
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