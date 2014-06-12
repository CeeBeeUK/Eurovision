using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.ComponentModel.DataAnnotations;


namespace Eurovision.Models
{
    public class Profile : ProfileBase
    {
        [Required, Display(Name = "Display Name")]
        public virtual string DisplayName
        {
            get
            {
                return (this.GetPropertyValue("DisplayName").ToString());
            }
            set
            {
                this.SetPropertyValue("DisplayName", value);
            }
        }
        public virtual bool ShowNotes
        {
            get
            {
                return ((bool)this.GetPropertyValue("ShowNotes"));
            }
            set
            {
                this.SetPropertyValue("ShowNotes", value);
            }
        }

        [Display(Name = "Current Game")]
        public virtual int CurrentGame
        {
            get
            {
                return ((int)this.GetPropertyValue("CurrentGame"));
            }
            set
            {
                this.SetPropertyValue("CurrentGame", value);
            }
        }
        public static Profile GetProfile(string username)
        {
            return Create(username) as Profile;
        }
    }
}