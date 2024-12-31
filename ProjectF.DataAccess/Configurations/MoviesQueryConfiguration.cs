using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectF.DataAccess.Entities;

namespace ProjectF.DataAccess.Configurations;

public class MoviesQueryConfiguration : IEntityTypeConfiguration<MoviesQueryEntity>
{
    public void Configure(EntityTypeBuilder<MoviesQueryEntity> builder)
    {
        builder.ToTable("MoviesQuery").HasKey(e => e.Id);

        builder.Property(e => e.Term).HasMaxLength(100);
    }
}