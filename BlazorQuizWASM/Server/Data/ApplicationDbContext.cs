using BlazorQuizWASM.Server.Models;
using BlazorQuizWASM.Server.Models.Domain;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace BlazorQuizWASM.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<MediaType>? MediaTypes { get; set; }
        public DbSet<MediaFile>? MediaFiles { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<Answer>? Answers { get; set; }
        public DbSet<QuizItem>? QuizItems { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MediaType>()
               .HasIndex(u => u.Mediatype)
               .IsUnique();

            modelBuilder.Entity<MediaFile>()
               .HasIndex(u => u.MediaFileName)
               .IsUnique();


            modelBuilder.Entity<MediaFile>()
            .HasOne(u => u.MediaTypes)
            .WithMany(a => a.MediaFiles)
            .HasForeignKey(u => u.FkMediaTypeId);

            modelBuilder.Entity<Question>()
            .HasOne(u => u.ApplicationUsers)
            .WithMany(a => a.Questions)
            .HasForeignKey(u => u.FkUserId);

            modelBuilder.Entity<Question>()
           .HasOne(u => u.MediaFiles)
           .WithMany(a => a.Questions)
           .HasForeignKey(u => u.FkFileId);

            modelBuilder.Entity<Answer>()
            .HasOne(u => u.Questions)
            .WithMany(a => a.Answers)
            .HasForeignKey(u => u.FkQuestionId);

            modelBuilder.Entity<QuizItem>()
            .HasOne(u => u.Questions)
            .WithMany(a => a.QuizItems)
            .HasForeignKey(u => u.FkQuestionId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<QuizItem>()
            .HasOne(u => u.ApplicationUsers)
            .WithMany(a => a.QuizItems)
            .HasForeignKey(u => u.FkUserId);

        }
    }
}