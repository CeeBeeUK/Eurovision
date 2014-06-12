using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eurovision.Models
{
    public class CreateEventVM
    {
        public Event Event { get; set; }
        public SelectList Countries { get; set; }
    }
}