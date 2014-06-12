using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Eurovision.Models
{
    public class EventVM
    {
        public Event Event { get; set; }
        public IEnumerable<EventCountry> EventCountries { get; set; }
    }
    public class EventWinnerVM
    {
        public Event Event { get; set; }
        //public EurovisionWinner EurovisionWinner { get; set; }
        //public HomeChampion HomeChampion { get; set; }
        [Display(Name="Winning country")]
        public int EventCountryEuroWinner { get; set; }
        [Display(Name="Home Champ")]
        public int EventCountryHomeChamp { get; set; }
        public SelectList EventCountries { get; set; }
        //public SelectList Countries { get; set; }
        //public SelectList Players { get; set; }
    }

}