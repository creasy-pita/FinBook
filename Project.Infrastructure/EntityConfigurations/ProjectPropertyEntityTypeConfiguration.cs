using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfigurations
{
    class ProjectPropertyEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectProperty> builder)
        {
            builder.ToTable("ProjectProperties")
            .HasKey(t => t.Key);
        }
    }
}

