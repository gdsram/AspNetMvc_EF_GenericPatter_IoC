namespace DTO
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Infrastructure;

    public partial class ManyToManyDBContext : DbContext, IDbContext
    {
        public ManyToManyDBContext()
            : base("name=ManyToManyDBContext")
        {
        }

        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<JobPost> JobPosts { get; set; }
        public virtual DbSet<JobTag> JobTags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employer>()
                .HasMany(e => e.JobPosts)
                .WithRequired(e => e.Employer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<JobPost>()
                .HasMany(e => e.JobTags)
                .WithMany(e => e.JobPosts)
                .Map(m => m.ToTable("JobPost_JobTag").MapLeftKey("JobPostID").MapRightKey("JobTagID"));
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity t) where TEntity : BaseEntity
        {
            return base.Entry<TEntity>(t);
        }
    }
}
