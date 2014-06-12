using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurovision.Models
{
    public class GameVM
    {
        public EventPlayer EventPlayer { get; set; }
        public Event Event { get; set; }
        public IEnumerable<EventCountry> ParticipatingCountries { get; set; }
        public IEnumerable<PlayerEventCountryScore> PlayerScores { get; set; }
    }
}