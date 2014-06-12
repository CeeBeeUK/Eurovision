using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Web.Profile;

namespace Eurovision.Models
{
    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Year { get; set; }
        [MaxLength(50)]
        public string HostCity { get; set; }
        public int CountryID { get; set; }
        public bool Active { get; set; }

        public int? EuroWinnerID { get; set; }
        public int? HomeChampID { get; set; }

        [ForeignKey("EuroWinnerID")]
        public virtual EurovisionWinner EurovisionWinner { get; set; }
        [ForeignKey("HomeChampID")]
        public virtual HomeChampion HomeChampion { get; set; }
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }
        public virtual ICollection<EventCountry> Countries { get; set; }

    }
}