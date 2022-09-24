using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Tests.Fixtures
{
    public static class TeamFixture
    {
        public static List<TeamDto> GetAll() =>
            new()
            {
                new TeamDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test name 1",
                    CoachName = "Test coach name 1"
                },
                new TeamDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test name 2",
                    CoachName = "Test coach name 2"
                },
                new TeamDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test name 3",
                    CoachName = "Test coach name 3"
                }
            };
    }
}
