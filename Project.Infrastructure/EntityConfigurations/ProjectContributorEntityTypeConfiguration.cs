using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfigurations
{
    class ProjectContributorEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectContributor> builder)
        {
            builder.ToTable("ProjectContributors")
            .HasKey(t => t.Id);
        }
    }
}
