using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Projects;

namespace TaskManager.Repository.Data.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.Property(p => p.ClientName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.DesignCost)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ProductionCost)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            
            builder.Property(p => p.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();
        }
    }
}
