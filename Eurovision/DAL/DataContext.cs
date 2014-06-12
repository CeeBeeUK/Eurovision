using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Eurovision.Models;

namespace Eurovision.DAL
{
    public class DataContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<PlayerEventCountryScore> PlayerScores { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCountry> EventCountries { get; set; }
        public DbSet<EventPlayer> EventPlayers { get; set; }
        public DbSet<HomeChampion> HomeChampions { get; set; }
        public DbSet<EurovisionWinner> EurovisionWinners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                        .HasRequired(a => a.Country)
                        .WithMany()
                        .HasForeignKey(u => u.CountryID)
                        .WillCascadeOnDelete(false);
        }
    }
}