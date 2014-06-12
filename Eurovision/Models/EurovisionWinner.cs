using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Web.Profile;

namespace Eurovision.Models
{
    public class EurovisionWinner
    {
        [Key]
        public int id { get; set; }
        public int Year { get; set; }
        public int CountryID { get; set; }
        public Guid Player { get; set; }

        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }

        public virtual string WonBy
        {
            get
            {
                string result = "";
                using (Eurovision.DAL.DataContext db = new DAL.DataContext())
                {
                    string WinCountry = "";
                    string WinPlayer = "";
                    WinCountry = Country.Name;
                    MembershipUser user = Membership.GetUser(Player);
                    if (user != null)
                    {
                        ProfileBase profile = Profile.GetProfile(user.UserName);
                        WinPlayer = string.Format("{0} with ", (string)profile.GetPropertyValue("DisplayName"));
                    }
                    result = string.Format("Won by {1}{0}", WinCountry, WinPlayer);
                }
                
                return result;
            }
        }

    }
}