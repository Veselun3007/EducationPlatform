using CourseContent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseContent.Infrastructure.Configurations
{
    internal class MaterialfileConfiguration : IEntityTypeConfiguration<Materialfile>
    {
        public void Configure(EntityTypeBuilder<Materialfile> builder)
        {
            builder.HasKey(e => e.Id).HasName("materialfiles_pkey");
            builder.ToTable("material_files");

            builder.Property(e => e.Id).HasColumnName("material_file_id");
            builder.Property(e => e.MaterialFile).HasColumnType("character varying").HasColumnName("material_file");
            builder.Property(e => e.MaterialId).ValueGeneratedOnAdd().HasColumnName("material_id");

            builder.HasOne(d => d.Material).WithMany(p => p.Materialfiles).HasForeignKey(d => d.MaterialId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_materialfiles_materials");
        }
    }
}
