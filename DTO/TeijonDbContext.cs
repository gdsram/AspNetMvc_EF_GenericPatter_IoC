namespace DTO
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TeijonDbContext : DbContext, IDbContext
    {
        public TeijonDbContext()
            : base("name=TeijonModel")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.people)
                .WithOptional(e => e.job)
                .HasForeignKey(e => e.id_job);

            modelBuilder.Entity<Person>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.sex)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.description)
                .IsUnicode(false);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
    }
}
