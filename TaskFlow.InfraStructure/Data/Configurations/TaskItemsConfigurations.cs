using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.Entites;

namespace TaskFlow.InfraStructure.Data.Configurations
{
    public class TaskItemsConfigurations : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.Property(t => t.Status)
               .HasConversion<string>();

            builder.Property(t => t.Priority)
               .HasConversion<string>();
        }
    }
}
