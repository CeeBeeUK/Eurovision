using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace Eurovision.Models
{
    public class EventPlayer
    {
        [Key]
        public int id { get; set; }
        public int Year { get; set; }
        public Guid PlayerGuid { get; set; }

        [Display(Name="Predicted UK Score")]
        public int? PredictedUKScore { get; set; }

        [ForeignKey("Year")]
        public virtual Event Event { get; set; }

        public virtual string Name
        {
            get
            {
                string result = "Not yet allocated";
                var owner = Membership.GetUser(PlayerGuid);
                if (owner == null) return result;
                var ownerProfile = Profile.GetProfile(owner.UserName);
                if (ownerProfile == null) return owner.UserName;
                return ownerProfile.DisplayName;
            }
        }

    }
}