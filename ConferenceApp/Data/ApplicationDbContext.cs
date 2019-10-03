using System;
using System.Collections.Generic;
using System.Text;
using ConferenceApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConferenceApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<ConferenceVersion> ConferenceVersions { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}