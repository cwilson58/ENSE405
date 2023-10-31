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

    public virtual DbSet<DiaryEntry> DiaryEntries { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MealFood> MealFoods { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DiaryEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__DiaryEnt__F57BD2D7EF591DFC");

            entity.Property(e => e.EntryId).HasColumnName("EntryID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
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
            entity.HasKey(e => e.MealId).HasName("PK__Meals__ACF6A65D71956D35");

            entity.Property(e => e.MealId).HasColumnName("MealID").ValueGeneratedOnAdd();
            entity.Property(e => e.EntryId).HasColumnName("EntryID");
            entity.Property(e => e.MealNumber).HasColumnName("MealNumber").HasColumnType("tinyint");
        });

        modelBuilder.Entity<MealFood>(entity =>
        {
            entity.HasKey(e => e.MealFoodId);

            entity.Property(e => e.MealFoodId).HasColumnName("MealFoodId").ValueGeneratedOnAdd();

            entity.Property(e => e.Barcode)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.MealId).HasColumnName("MealID");
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
