using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Project.Domain.SeedWork;
using Project.Infrastructure.EntityConfigurations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class ProjectContext : DbContext, IUnitOfWork
    {

        private readonly IMediator _mediator;

        public DbSet<Domain.AggregatesModel.Project> Projects { get; set; }

        private ProjectContext(DbContextOptions<ProjectContext> options) : base (options) { }

        public ProjectContext(DbContextOptions<ProjectContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("ProjectContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Domain.AggregatesModel.Project>(b => 
            //        b.ToTable("Projects")
            //        .HasKey(t => t.Id)
            //    );
            //改用ApplyConfiguration 方法配置
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectContributorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectPropertyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectViewerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVisibleRulesEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration()); 
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);//在 MediatorExtension 扩展 _mediator 来实现

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed throught the DbContext will be commited
            var result = await base.SaveChangesAsync();

            return true;
        }        
    }

}
