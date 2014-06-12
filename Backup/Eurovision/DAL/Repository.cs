using System;
using System.Data;
using System.Linq;
using Eurovision.DAL;
using Eurovision.Models;
using System.Collections.Generic;

namespace Eurovision
{
    public interface SourceRepository : IDisposable
    {
        IQueryable<Country> GetAllCountries();
        Country getCountryByID(int id);
        IQueryable<Event> GetAllEvents();
        Event GetEventByYear(int year);
        void CreateEvent(Event c);
        IQueryable<EventCountry> GetEventCountriesByYear(int year);
        void AddCountryToEvent(AddCountryVM acvm);
        IQueryable<Event> ListOpenEvents();
        IQueryable<Event> ListOpenEventsPlayerCanJoin(Guid PlayerGuid);
        void StartGameForPlayer(ICollection<PlayerEventCountryScore> pecs);
        void AddEventPlayer(EventPlayer model);
        EventPlayer GetCurrentEventPlayer(int year, Guid guid);
        int? GetUserPredictionForYear(int id, Guid guid);
        IEnumerable<PlayerEventCountryScore> GetPlayerScoresForYear(int p, Guid g);
        void RecordPlayerScore(PlayerEventCountryScore model);
        EventCountry GetEventCountry(int id);
        PlayerEventCountryScore GetPlayerScoreByID(int p);
        IEnumerable<EventPlayer> GetPlayersForYear(int p);

        void AllocateCountryToPlayer(int id, Guid p);
    }

    public class SQLRepository : SourceRepository
    {
        DataContext db = new DataContext();
        #region Implements
        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion Implements
        #region Countries
        IQueryable<Country> SourceRepository.GetAllCountries()
        {
            return db.Countries;
        }
        Country SourceRepository.getCountryByID(int id)
        {
            return db.Countries.Find(id);
        }
        #endregion Countries
        #region Events
        IQueryable<Event> SourceRepository.GetAllEvents()
        {
            return db.Events;
        }
        IQueryable<Event> SourceRepository.ListOpenEvents()
        {
            return db.Events.Where(x => x.Active == true);
        }
        IQueryable<Event> SourceRepository.ListOpenEventsPlayerCanJoin(Guid PlayerGuid)
        {
            return db.Events.Where(x => x.Active == true);
        }
        void SourceRepository.CreateEvent(Event model)
        {
            model.Active = true;
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }
        Event SourceRepository.GetEventByYear(int year)
        {
            return db.Events.Single(x => x.Year == year);
        }
        #endregion Events
        #region EventCountries
        IQueryable<EventCountry> SourceRepository.GetEventCountriesByYear(int year)
        {
            return db.EventCountries.Where(x => x.EventID == year);
        }
        void SourceRepository.AddCountryToEvent(AddCountryVM acvm)
        {
            int newSequence = 1;
            
            var exist = db.EventCountries.Where(x => x.EventID == acvm.Year);
            if (exist != null && exist.Count() > 0) 
            {
                newSequence = exist.Max(x => x.Sequence) + 1;
            }
            EventCountry newCC = new EventCountry{ CountryID=acvm.CountryID, EventID=acvm.Year, Sequence=newSequence};
            db.Entry(newCC).State = EntityState.Added;
            db.SaveChanges();
        }
        EventCountry SourceRepository.GetEventCountry(int id)
        {
            return db.EventCountries.Find(id);
        }

        public void AllocateCountryToPlayer(int id, Guid p)
        {
            EventCountry EC = db.EventCountries.Find(id);
            EC.OwningPlayer = p;
            db.Entry(EC).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion EventCountries
        #region EventPlayers
        void SourceRepository.AddEventPlayer(EventPlayer model)
        {
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }
        EventPlayer SourceRepository.GetCurrentEventPlayer(int year, Guid guid)
        {
            return db.EventPlayers.Where(x => x.Year == year && x.PlayerGuid == guid).SingleOrDefault();
        }

        int? SourceRepository.GetUserPredictionForYear(int id, Guid guid)
        {
            EventPlayer ep = db.EventPlayers.Where(x => x.PlayerGuid == guid && x.Year == id).FirstOrDefault();
            if (ep != null)
            {
                return ep.PredictedUKScore;
            }
            return null;
        }
        void SourceRepository.StartGameForPlayer(ICollection<PlayerEventCountryScore> pecs)
        {
            foreach (var item in pecs)
            {
                db.PlayerScores.Add(item);
            }
            db.SaveChanges();
        }
        IEnumerable<EventPlayer> SourceRepository.GetPlayersForYear(int p)
        {
            return db.EventPlayers.Where(x => x.Year == p);
        }

        #endregion EventPlayers
        #region PlayerScores
        IEnumerable<PlayerEventCountryScore> SourceRepository.GetPlayerScoresForYear(int p, Guid g)
        {
            return db.PlayerScores
                    .Where(x => x.EventCountry.Event.Year == p && x.PlayerGuid == g)
                    .OrderBy(x => (x.Score == 0 || x.Score == null) ? x.EventCountry.Sequence : x.EventCountry.Sequence + 11);
        }
        void SourceRepository.RecordPlayerScore(PlayerEventCountryScore model)
        {
            if (model.Fattest)
            {
                foreach (var ps in db.PlayerScores.Where(x => x.PlayerGuid == model.PlayerGuid && x.Fattest == true))
                {
                    ps.Fattest = false;
                } 
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }
        PlayerEventCountryScore SourceRepository.GetPlayerScoreByID(int p)
        {
            return db.PlayerScores.Find(p);
        }       
        #endregion PlayerScores




        void SourceRepository.AllocateCountryToPlayer(int id, Guid p)
        {
            throw new NotImplementedException();
        }
    }
}
