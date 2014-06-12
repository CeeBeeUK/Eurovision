using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace Eurovision.Models
{
    public class EventCountry
    {

        [Key]
        public int id { get; set; }
        public int EventID { get; set; }
        public int CountryID { get; set; }
        public int Sequence { get; set; }
        public int FinalRealScore { get; set; }
        public Guid OwningPlayer { get; set; }
        [ForeignKey("EventID")]
        public virtual Event Event { get; set; }
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }

        public virtual string CountryOwnedBy
        {
            get
            {
                string result = "Not yet allocated";
                var owner = Membership.GetUser(OwningPlayer);
                if (owner == null) return result;
                var ownerProfile = Profile.GetProfile(owner.UserName);
                if (ownerProfile == null) return owner.UserName;
                return ownerProfile.DisplayName;
            }
        }

    }
}