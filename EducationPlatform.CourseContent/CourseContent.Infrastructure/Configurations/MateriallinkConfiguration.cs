using CourseContent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseContent.Infrastructure.Configurations
{
    internal class MateriallinkConfiguration : IEntityTypeConfiguration<Materiallink>
    {
        public void Configure(EntityTypeBuilder<Materiallink> builder)
        {
            builder.HasKey(e => e.Id).HasName("material_links_pkey");
            builder.ToTable("material_links");

            builder.Property(e => e.Id).HasColumnName("material_link_id");
            builder.Property(e => e.MaterialLink).HasColumnType("character varying").HasColumnName("material_link");
            builder.Property(e => e.MaterialId).ValueGeneratedOnAdd().HasColumnName("material_id");

            builder.HasOne(d => d.Material).WithMany(p => p.Materiallinks).HasForeignKey(d => d.MaterialId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_materiallinks_materials");
        }
    }
}
