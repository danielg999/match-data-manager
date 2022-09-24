using FluentValidation;
using MatchDataManager.Api.Entities;

namespace MatchDataManager.Api.Models.Validations
{
    public class TeamUpdateDtoValidator : AbstractValidator<TeamUpdateDto>
    {
        private readonly MatchDataManagerDbContext _dbContext;

        public TeamUpdateDtoValidator(MatchDataManagerDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(t => t.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(t => t.CoachName)
                .MaximumLength(55);

            RuleFor(t => new { t.Name, t.Id })
                .Custom((value, context) =>
                {
                    var nameInUse = _dbContext.Teams.Any(t => t.Name == value.Name && t.Id != value.Id);

                    if (nameInUse)
                    {
                        context.AddFailure("Name", "That name is taken.");
                    }
                });
        }
    }
}