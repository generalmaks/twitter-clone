using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBack.Dal.Post.Dao;
using TwitterCloneBack.Dal.User.Dao;

namespace TwitterCloneBack.Dal;

public partial class TwitterCloneContext : DbContext
{
    public TwitterCloneContext()
    {
    }

    public TwitterCloneContext(DbContextOptions<TwitterCloneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PostDao> Posts { get; set; }

    public virtual DbSet<UserDao> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostDao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Posts__3214EC074E84AA35");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Author).WithMany(p => p.Posts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Posts_To_Users");
        });

        modelBuilder.Entity<UserDao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07EB2A0E6C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
