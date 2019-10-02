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
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackScope> FeedbackScopes { get; set; }
        public DbSet<FeedbackCategory> FeedbackCategories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
