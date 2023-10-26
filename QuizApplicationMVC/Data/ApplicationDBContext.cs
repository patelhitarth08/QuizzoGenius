using Microsoft.EntityFrameworkCore;
using QuizApplicationMVC.Models;

namespace QuizApplicationMVC.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<QuizQuestion> QuizQuestion { get; set; }
        public DbSet<QuizUserHistory> QuizUserHistory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Questions>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(qq => qq.QuizId);

            modelBuilder.Entity<QuizUserHistory>()
                .HasOne(q => q.User)
                .WithMany(u => u.quizUserHistory)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuizUserHistory>()
                .HasOne(q => q.Quiz)
                .WithMany(u => u.quizUserHistory)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}