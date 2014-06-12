using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Eurovision.Models
{
    public class AddCountryVM
    {
        public int Year { get; set; }
        [Display(Name="Country")]
        public int CountryID { get; set; }
        public SelectList Countries { get; set; }
    }
}
