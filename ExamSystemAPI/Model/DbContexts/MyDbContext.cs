﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamSystemAPI.Model.DbContexts
{
    public class MyDbContext : IdentityDbContext<User, Role, long>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Paper> Papers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ExamRecord> ExamRecords { get; set; }
        public DbSet<ErrorRecord> ErrorRecords { get; set; }
        public DbSet<PaperTeam> PaperTeams { get; set; }
        public DbSet<PaperTopic> PaperTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

    }
}
