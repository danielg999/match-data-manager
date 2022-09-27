using FluentValidation;
using MatchDataManager.Api.Repository;

namespace MatchDataManager.Api.Models.Validations
{
    public class TeamUpdateDtoValidator : AbstractValidator<TeamUpdateDto>
    {
        private readonly ITeamRepository _teamRepository;

        public TeamUpdateDtoValidator(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;

            RuleFor(t => t.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(t => t.CoachName)
                .MaximumLength(55);

            RuleFor(t => new { t.Name, t.Id })
                .Custom(async (value, context) =>
                {
                    var nameInUse = await _teamRepository.IsAnyExistOnUpdate(value.Name, value.Id);

                    if (nameInUse)
                    {
                        context.AddFailure("Name", "That name is taken.");
                    }
                });
        }
    }
}