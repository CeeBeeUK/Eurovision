using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Web.Profile;

namespace Eurovision.Models
{
    public class HomeChampion
    {
        [Key]
        public int id { get; set; }
        public int Year { get; set; }
        public int CountryID { get; set; }
        public Guid Player { get; set; }

        //[ForeignKey("CountryID")]
        //public virtual Country Country { get; set; }

        public virtual string WonBy
        {
            get
            {
                string result = "";
                using (Eurovision.DAL.DataContext db = new DAL.DataContext())
                {
                    string WinCountry = "";
                    string WinPlayer = "";
                    WinCountry = db.Countries.Find(CountryID).Name;
                    MembershipUser user = Membership.GetUser(Player);
                    if (user != null)
                    {
                        ProfileBase profile = Profile.GetProfile(user.UserName);
                        WinPlayer = string.Format("was {0} with ", (string)profile.GetPropertyValue("DisplayName"));
                    }
                    result = string.Format(" Home champion {1}{0}", WinCountry, WinPlayer);
                }

                return result;
            }
        }

    }
}