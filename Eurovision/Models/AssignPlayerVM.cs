using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Eurovision.Models
{
    public class AssignPlayerVM
    {
        public EventCountry EventCountry { get; set; }
        public Guid PlayerGuid { get; set; }
        public SelectList Players { get; set; }
    }
}
