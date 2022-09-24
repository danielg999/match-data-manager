using FluentValidation;
using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Models.Validations
{
    public class LocationCreateDtoValidator : AbstractValidator<LocationCreateDto>
    {
        private readonly MatchDataManagerDbContext _dbContext;

        public LocationCreateDtoValidator(MatchDataManagerDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(l => l.Name)
                .NotEmpty()
                .MaximumLength(255)
                .Custom((value, context) =>
                {
                    var nameInUse = _dbContext.Locations.Any(l => l.Name == value);
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