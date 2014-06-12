using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurovision.Models
{
    public class EventVM
    {
        public Event Event { get; set; }
        public IEnumerable<EventCountry> EventCountries { get; set; }
    }
}