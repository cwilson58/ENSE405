using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EatingHabitAnalyzerAPI.Models;

public partial class EatingHabitAnalyzerContext : DbContext
{
    public EatingHabitAnalyzerContext()
    {
    }

    public EatingHabitAnalyzerContext(DbContextOptions<EatingHabitAnalyzerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DirayEntry> DirayEntries { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MealFood> MealFoods { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DirayEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__DirayEnt__F57BD2D74549708B");

            entity.Property(e => e.EntryId).HasColumnName("EntryID");
            entity.Property(e => e.EntryDateTime).HasPrecision(2);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.DirayEntries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DirayEntr__UserI__73BA3083");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("PK__Foods__177800D2CEF4BC96");

            entity.Property(e => e.Barcode)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.FoodName)
                .HasMaxLength(125)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meals__ACF6A65D302AE4D6");

            entity.Property(e => e.MealId).HasColumnName("MealID");
            entity.Property(e => e.EntryId).HasColumnName("EntryID");

            entity.HasOne(d => d.Entry).WithMany(p => p.Meals)
                .HasForeignKey(d => d.EntryId)
                .HasConstraintName("FK__Meals__EntryID__76969D2E");
        });

        modelBuilder.Entity<MealFood>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Barcode)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.MealId).HasColumnName("MealID");

            entity.HasOne(d => d.BarcodeNavigation).WithMany()
                .HasForeignKey(d => d.Barcode)
                .HasConstraintName("FK__MealFoods__Barco__797309D9");

            entity.HasOne(d => d.Meal).WithMany()
                .HasForeignKey(d => d.MealId)
                .HasConstraintName("FK__MealFoods__MealI__787EE5A0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC280950CF");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.DateOfBirth).HasPrecision(2);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GoalWeight).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pin)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.WeightInPounds).HasColumnType("decimal(5, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
