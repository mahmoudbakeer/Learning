using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScaffoldingInEF.Entities;

namespace ScaffoldingInEF.Data;

public partial class TechTalkDbContext : DbContext
{
    public TechTalkDbContext()
    {
    }

    public TechTalkDbContext(DbContextOptions<TechTalkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Speaker> Speakers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3214EC0775109D75");

            entity.HasOne(d => d.Speaker).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__SpeakerI__398D8EEE");
        });

        modelBuilder.Entity<Speaker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Speakers__3214EC072B0AB352");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
