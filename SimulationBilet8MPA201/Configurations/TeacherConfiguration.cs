using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimulationBilet8MPA201.Models;

namespace SimulationBilet8MPA201.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(256);
        builder.Property(x=>x.LastName).IsRequired().HasMaxLength(256);
    }
}
