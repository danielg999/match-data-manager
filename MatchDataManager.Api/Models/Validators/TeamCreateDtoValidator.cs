using FluentValidation;
using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Models.Validations
{
    public class TeamCreateDtoValidator : AbstractValidator<TeamCreateDto>
    {
        private readonly MatchDataManagerDbContext _dbContext;

        public TeamCreateDtoValidator(MatchDataManagerDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(t => t.Name)
                .NotEmpty()
                .MaximumLength(255)
                .Custom((value, context) =>
                {
                    var nameInUse = _dbContext.Teams.Any(t => t.Name == value);

                    if (nameInUse)
                    {
                        context.AddFailure("Name", "That name is taken.");
                    }
                });

            RuleFor(t => t.CoachName)
                .MaximumLength(55);
        }
    }
}