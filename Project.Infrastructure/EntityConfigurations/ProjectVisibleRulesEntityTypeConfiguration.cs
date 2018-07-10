using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfigurations
{
    class ProjectVisibleRulesEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectVisibleRules>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectVisibleRules> builder)
        {
            builder.ToTable("ProjectVisibleRules")
            .HasKey(t => t.Id);
        }
    }
}