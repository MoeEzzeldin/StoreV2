using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Store.Data.DbSets;
using Store.Models.Entities;

namespace Store.Data;

public partial class ApplicationDbContext : DbContext
{
    private ProductDbSet _productDbSet;
    private ReviewDbSet _reviewDbSet;
    private UserDbSet _userDbSet;
    
    public ProductDbSet ProductSet => _productDbSet ??= new ProductDbSet(this);
    public ReviewDbSet ReviewSet => _reviewDbSet ??= new ReviewDbSet(this);
    public UserDbSet UserSet => _userDbSet ??= new UserDbSet(this);
    
    // Original DbSet properties needed for EF Core infrastructure
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<User> Users { get; set; }

    // Constructor that will be used by the DI system - highest priority
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    // For design-time tools and migrations
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Nothing to do here, this is only for design-time tools
            // when no options have been configured
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Brand)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("brand");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PictureUrl)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("picture_url");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("review");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Reviewer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reviewer");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_review");

            entity.HasMany(d => d.Users).WithMany(p => p.Reviews)
                .UsingEntity<Dictionary<string, object>>(
                    "UserReview",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_user_id"),
                    l => l.HasOne<Review>().WithMany()
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_review_id"),
                    j =>
                    {
                        j.HasKey("ReviewId", "UserId");
                        j.ToTable("user_review");
                        j.IndexerProperty<int>("ReviewId").HasColumnName("review_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_user");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Salt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("salt");
            entity.Property(e => e.UserRole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
