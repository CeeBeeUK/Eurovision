using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Eurovision.Models
{
    public class ProfileVM
    {
        [Display(Name="Display Name")]
        public string DisplayName { get; set; }
        [Display(Name="Current Game")]
        public int CurrentGame { get; set; }
        public bool ShowNotes { get; set; }
    }
}