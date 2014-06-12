using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurovision.Models
{
    public class EventWithScoresVM
    {
        public string Country { get; set; }
        public double Score { get; set; }
        public int Votes { get; set; }
    }

}