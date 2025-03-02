using eSchoolDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace eSchoolDatabase.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {

            builder.HasOne(m => m.School)
                .WithMany(s => s.Managers)
                .HasForeignKey(m => m.SchoolId)
                .OnDelete(DeleteBehavior.Restrict); // Okul silindiğinde yöneticiler )
        }
    }
}
