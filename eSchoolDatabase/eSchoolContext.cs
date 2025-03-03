using eSchoolDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace eSchoolDatabase
{
    public class eSchoolContext : DbContext
    {
        public eSchoolContext(DbContextOptions<eSchoolContext> options) : base(options) { }
        public DbSet<School> Schools { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);  // Ogrenci vbir attendance a bagliysa silinemez.


            modelBuilder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Restrict); // Okul silindiğinde öğrenciler silinmez

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.School)
                .WithMany(s => s.Teachers)
                .HasForeignKey(t => t.SchoolId)
                .OnDelete(DeleteBehavior.Restrict); // Okul silindiğinde öğretmenler silinmez

        }


    }
}
