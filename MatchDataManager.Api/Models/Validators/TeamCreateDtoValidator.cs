using FluentValidation;
using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Repository;

namespace MatchDataManager.Api.Models.Validations
{
    public class TeamCreateDtoValidator : AbstractValidator<TeamCreateDto>
    {
        private readonly ITeamRepository _teamRepository;

        public TeamCreateDtoValidator(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;

            RuleFor(t => t.Name)
                .NotEmpty()
                .MaximumLength(255)
                .Custom(async (value, context) =>
                {
                    var nameInUse = await _teamRepository.IsAnyExistOnCreate(value);

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