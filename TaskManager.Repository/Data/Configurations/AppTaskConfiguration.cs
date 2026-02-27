using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Repository.Data.Configurations
{
    public class AppTaskConfiguration : IEntityTypeConfiguration<AppTask>
    {
        public void Configure(EntityTypeBuilder<AppTask> builder)
        {
            // Why explicit table name? -> Avoids EF pluralizing it to
            //                             "AppTasks" or conflicting with
            //                             any system-reserved "Tasks" name
            builder.ToTable("Tasks");

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.Team)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.Deadline)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            // AssignedTo relationship
            builder.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
            

            // CreatedBy relationship
            builder.HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
                               
        }
    }
}

