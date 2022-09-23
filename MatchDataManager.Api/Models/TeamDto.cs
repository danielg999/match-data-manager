using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchDataManager.Api.Models
{
    public class TeamDto : EntityDto
    {
        public string Name { get; set; }

        public string CoachName { get; set; }
    }
}
