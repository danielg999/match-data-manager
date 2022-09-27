using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchDataManager.Api.Models;

namespace MatchDataManager.Tests.Fixtures
{
    public static class LocationFixture
    {
        public static List<LocationDto> GetTestAll() =>
            new()
            {
                new LocationDto()
                {
                    City = "Poznań",
                    Id = Guid.NewGuid(),
                    Name = "Test 1"
                },
                new LocationDto()
                {
                    City = "Warszawa",
                    Id = Guid.NewGuid(),
                    Name = "Test 2"
                },
                new LocationDto()
                {
                    City = "Kraków",
                    Id = Guid.NewGuid(),
                    Name = "Test 3"
                }
            };
    }
}
