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
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<ConferenceVersion> ConferenceVersions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCentre> EventCentres { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FeedbackCategory> FeedbackCategories { get; set; }
        public DbSet<FeedbackScope> FeedbackScopes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FoodService> FoodServices { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<PracticalSession> PracticalSessions { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Talk> Talks { get; set; }
        
        public DbSet<EventTag> EventTags { get; set; }
        
        public DbSet<CheckBoxItem> CheckBoxItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ConferenceApp.Models.EventCentre> EventCentre { get; set; }
    }
}
