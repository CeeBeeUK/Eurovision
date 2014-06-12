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
                new Models.HomeChampion { Year=2010, CountryID = 12, Player = new Guid("3D333908-E36E-4CE6-90A3-4B0E30F3801D")},//Helen
                new Models.HomeChampion { Year=2011, CountryID = 23, Player = new Guid("25B08002-BFEF-4CA1-B491-83381054822D")},//Joanne
                new Models.HomeChampion { Year=2012, CountryID = 19, Player = new Guid("9F8BDA81-209B-4640-AE6F-145122B47681")},//Onge
                //new Models.HomeChampion { Year=2013, CountryID = 1, Player = new Guid("00000000-0000-0000-0000-000000000000")},//unkown!
            };
            HC.ForEach(c => context.HomeChampions.Add(c));
            var EC = new List<EurovisionWinner>
            {
                new EurovisionWinner { Year=2010, CountryID=13, Player= new Guid("9F8BDA81-209B-4640-AE6F-145122B47681")},//Onge
                new EurovisionWinner { Year=2011, CountryID=4, Player= new Guid("00000000-0000-0000-0000-000000000000")},//Joe
                new EurovisionWinner { Year=2012, CountryID=33, Player= new Guid("00000000-0000-0000-0000-000000000000")},//Ros
                new EurovisionWinner { Year=2013, CountryID=7, Player= new Guid("25B08002-BFEF-4CA1-B491-83381054822D")},//Joanne
            };
            EC.ForEach(c => context.EurovisionWinners.Add(c));


            
            var ec = new List<EventCountry>
            {
                new EventCountry { CountryID=7, EventID=2013,Sequence=1},


                new EventCountry { CountryID=36,EventID=2014,Sequence=1},
                new EventCountry { CountryID=5, EventID=2014,Sequence=2},
                new EventCountry { CountryID=4, EventID=2014,Sequence=3},
                new EventCountry { CountryID=16,EventID=2014,Sequence=4},
                new EventCountry { CountryID=25,EventID=2014,Sequence=5},
                new EventCountry { CountryID=28,EventID=2014,Sequence=6},
                new EventCountry { CountryID=2, EventID=2014,Sequence=7},
                new EventCountry { CountryID=24,EventID=2014,Sequence=8},
                new EventCountry { CountryID=26,EventID=2014,Sequence=9},
                new EventCountry { CountryID=14,EventID=2014,Sequence=10},
                new EventCountry { CountryID=3, EventID=2014,Sequence=11},
                new EventCountry { CountryID=13,EventID=2014,Sequence=12},
                new EventCountry { CountryID=33,EventID=2014,Sequence=13},
                new EventCountry { CountryID=11,EventID=2014,Sequence=14},
                new EventCountry { CountryID=29,EventID=2014,Sequence=15},
                new EventCountry { CountryID=19,EventID=2014,Sequence=16},
                new EventCountry { CountryID=31,EventID=2014,Sequence=17},
                new EventCountry { CountryID=10,EventID=2014,Sequence=18},
                new EventCountry { CountryID=32,EventID=2014,Sequence=19},
                new EventCountry { CountryID=34,EventID=2014,Sequence=20},              
                new EventCountry { CountryID=15,EventID=2014,Sequence=21},
                new EventCountry { CountryID=22,EventID=2014,Sequence=22},
                new EventCountry { CountryID=7, EventID=2014,Sequence=23},
                new EventCountry { CountryID=35,EventID=2014,Sequence=24},
                new EventCountry { CountryID=30,EventID=2014,Sequence=25},
                new EventCountry { CountryID=37,EventID=2014,Sequence=26},

            };
            ec.ForEach(c => context.EventCountries.Add(c));
            context.SaveChanges();




            var Oslo = context.Events.Find(2010);
            Oslo.EuroWinnerID = 1;
            Oslo.HomeChampID = 1;
            var Düsseldorf = context.Events.Find(2011);
            Düsseldorf.EuroWinnerID = 2;
            Düsseldorf.HomeChampID = 2;
            var Baku = context.Events.Find(2012);
            Baku.EuroWinnerID = 3;
            Baku.HomeChampID = 3;
            var Malmo = context.Events.Find(2013);
            Malmo.EuroWinnerID = 4;
  
            context.SaveChanges();

            context.Database.ExecuteSqlCommand("ALTER TABLE dbo.EventCountries ADD CONSTRAINT UQ_Sequence UNIQUE (EventID, CountryID, Sequence)");
            context.Database.ExecuteSqlCommand("ALTER TABLE dbo.PlayerEventCountryScores ADD CONSTRAINT UQ_PlayerCountry UNIQUE (EventCountryID, PlayerGuid)");
            context.Database.ExecuteSqlCommand("ALTER TABLE dbo.EventPlayers ADD CONSTRAINT UQ_EventPlayer UNIQUE (Year, PlayerGuid)");
        }
    }
}

