using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Web.Mvc;

namespace Eurovision.Models
{
    public class PlayerEventCountryScore
    {
        [Key]
        public int id { get; set; }
        public Guid PlayerGuid { get; set; }
        public int EventCountryID { get; set; }
        [ClampDouble(0,12,ErrorMessage="Score must be between 0 and 12")]
        public double? Score { get; set; }
        [Display(Name="Best wail?")]
        public bool BestWail { get; set; }
        [Display(Name = "Fattest performer?")]
        public bool Fattest { get; set; }
        [Display(Name = "Wackiest act?")]
        public bool Wackiest { get; set; }
        [MaxLength(4000), DataType(DataType.MultilineText), UIHint("TextAreaWithCountdown")]
        [AdditionalMetadata("maxLength", 4000), Display(Name = "Note")]
        public string Notes { get; set; }
        [ForeignKey("EventCountryID")]
        public virtual EventCountry EventCountry { get; set; }
    }
}