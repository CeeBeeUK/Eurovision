using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Eurovision.Models;

namespace Eurovision.DAL
{
    public class DBInitialiser : DropCreateDatabaseAlways<DataContext> // DropCreateDatabaseIfModelChanges<DataContext> // CreateDatabaseIfNotExists<DataContext> // 
    {
        protected override void Seed(DataContext context)
        {
            #region countries
            var cnt = new List<Country>
            {
                new Country { Name = "Albania" },
                new Country { Name = "Armenia" },
                new Country { Name = "Austria" },
                new Country { Name = "Azerbaijan" },
                new Country { Name = "Belarus"},
                new Country { Name = "Belgium"},
                new Country { Name = "Denmark"},
                new Country { Name = "Estonia"},
                new Country { Name = "FYR Macedonia"},
                new Country { Name = "Finland"},
                new Country { Name = "France"},
                new Country { Name = "Georgia"},
                new Country { Name = "Germany"},
                new Country { Name = "Greece"},
                new Country { Name = "Hungary"},
                new Country { Name = "Iceland"},
                new Country { Name = "Ireland"},
                new Country { Name = "Israel"},
                new Country { Name = "Italy"},
                new Country { Name = "Latvia"},
                new Country { Name = "Lithuania"},
                new Country { Name = "Malta"},
                new Country { Name = "Moldova"},
                new Country { Name = "Montenegro"},
                new Country { Name = "Norway"},
                new Country { Name = "Poland"},
                new Country { Name = "Portugal"},
                new Country { Name = "Romania"},
                new Country { Name = "Russia"},
                new Country { Name = "San Marino"},
                new Country { Name = "Slovenia"},
                new Country { Name = "Spain"},
                new Country { Name = "Sweden"},
                new Country { Name = "Switzerland"},
                new Country { Name = "The Netherlands"},
                new Country { Name = "Ukraine"},
                new Country { Name = "United Kingdom"},
            };
            #endregion Countries
            cnt.ForEach(c => context.Countries.Add(c));

            var e = new List<Event>
            {
                new Event{ Active=true, HostCity="Copenhagen", Year=2014, CountryID=7},
                new Event{ Active=true, HostCity="Malmo", Year=2013, CountryID=33},
                new Event{ Active=true, HostCity="Baku", Year=2012, CountryID=4},
                new Event{ Active=true, HostCity="Düsseldorf", Year=2011, CountryID=13},
                new Event{ Active=true, HostCity="Oslo", Year=2010, CountryID=25},
            };
            e.ForEach(c => context.Events.Add(c));
            context.SaveChanges();

            var HC = new List<HomeChampion> {
                new Models.HomeChampion { Year=2010, CountryID = 12, Player = new Guid("82839235-759D-4C10-B6FC-300AFF6E121E")},//Helen
                new Models.HomeChampion { Year=2011, CountryID = 23, Player = new Guid("DF45421C-4D00-4AAC-8BE0-E4B1B96649DA")},//Joanne
                new Models.HomeChampion { Year=2012, CountryID = 19, Player = new Guid("E673CEAD-9D6D-4F2D-8397-222CF706BFB1")},//Onge
                //new Models.HomeChampion { Year=2013, CountryID = 1, Player = new Guid("DF45421C-4D00-4AAC-8BE0-E4B1B96649DA")},//
            };
            //HC.ForEach(c => context.HomeChampions.Add(c));
            var EC = new List<EurovisionWinner>
            {
                new EurovisionWinner { Year=2010, CountryID=13, Player= new Guid("E673CEAD-9D6D-4F2D-8397-222CF706BFB1")},//Onge
                new EurovisionWinner { Year=2011, CountryID=4, Player= new Guid("F060E658-8C25-4937-A654-2B0E0C0D1AE2")},//Joe
                new EurovisionWinner { Year=2012, CountryID=33, Player= new Guid("0CC2B445-E041-47E7-8730-CFD033F58BE6")},//Ros
                new EurovisionWinner { Year=2013, CountryID=7, Player= new Guid("DF45421C-4D00-4AAC-8BE0-E4B1B96649DA")},//Joanne
            };
            //EC.ForEach(c => context.EurovisionWinners.Add(c));


            
            var ec = new List<EventCountry>
            {
                new EventCountry { CountryID=7, EventID=2013,Sequence=1},
                new EventCountry { CountryID=7, EventID=2014,Sequence=1},
                new EventCountry { CountryID=11,EventID=2014,Sequence=2},
                new EventCountry { CountryID=13,EventID=2014,Sequence=3},
                new EventCountry { CountryID=19,EventID=2014,Sequence=4},
                new EventCountry { CountryID=32,EventID=2014,Sequence=5},
                new EventCountry { CountryID=37,EventID=2014,Sequence=6},
            };
            ec.ForEach(c => context.EventCountries.Add(c));
            context.SaveChanges();

            //var Oslo = context.Events.Find(2010);
            //Oslo.EuroWinnerID = 1;
            //Oslo.HomeChampID = 1;
            //var Düsseldorf = context.Events.Find(2011);
            //Düsseldorf.EuroWinnerID = 2;
            //Düsseldorf.HomeChampID = 2;
            //var Baku = context.Events.Find(2012);
            //Baku.EuroWinnerID = 3;
            //Baku.HomeChampID = 3;
            //var Malmo= context.Events.Find(2013);
            //Malmo.EuroWinnerID = 4;
  
            context.SaveChanges();

            context.Database.ExecuteSqlCommand("ALTER TABLE dbo.EventCountries ADD CONSTRAINT UQ_Sequence UNIQUE (EventID, CountryID, Sequence)");
            context.Database.ExecuteSqlCommand("ALTER TABLE dbo.PlayerEventCountryScores ADD CONSTRAINT UQ_PlayerCountry UNIQUE (EventCountryID, PlayerGuid)");
            context.Database.ExecuteSqlCommand("ALTER TABLE dbo.EventPlayers ADD CONSTRAINT UQ_EventPlayer UNIQUE (Year, PlayerGuid)");
        }
    }
}

