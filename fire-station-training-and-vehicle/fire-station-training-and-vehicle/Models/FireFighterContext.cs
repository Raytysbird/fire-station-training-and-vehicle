﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace fire_station_training_and_vehicle.Models
{
    public partial class FireFighterContext : DbContext
    {
        public FireFighterContext()
        {
        }

        public FireFighterContext(DbContextOptions<FireFighterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<AssignedEvent> AssignedEvents { get; set; } = null!;
        public virtual DbSet<AssignedTask> AssignedTasks { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<IssueType> IssueTypes { get; set; } = null!;
        public virtual DbSet<Maintenance> Maintenances { get; set; } = null!;
        public virtual DbSet<RequestType> RequestTypes { get; set; } = null!;
        public virtual DbSet<Station> Stations { get; set; } = null!;
        public virtual DbSet<UserTask> UserTasks { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;
        public virtual DbSet<VehicleCatalogue> VehicleCatalogues { get; set; } = null!;
        public virtual DbSet<VehicleReport> VehicleReports { get; set; } = null!;
        public virtual DbSet<VehicleType> VehicleTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=KANGPC\\SQLEXPRESS19;Database=firefighters;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspNetUsers_ToTable");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AssignedEvent>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.UserId })
                    .HasName("PK__Assigned__EF6D3693B64FF9AD");

                entity.ToTable("AssignedEvent");

                entity.Property(e => e.EventId).HasColumnName("Event_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.Grade)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.AssignedEvents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKEvent_Fire126288");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AssignedEvents)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKEvent_Fire660096");
            });

            modelBuilder.Entity<AssignedTask>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.UserId })
                    .HasName("PK__Assigned__636993FAE1C363EF");

                entity.ToTable("AssignedTask");

                entity.Property(e => e.TaskId).HasColumnName("Task_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.AssignedTasks)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKTask_FireF95093");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AssignedTasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKTask_FireF109412");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Details)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RenewalPeriod)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Path).IsUnicode(false);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.RequestTypeId)
                    .HasConstraintName("FK_Document_ToTYpe");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Document_ToTable");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TeacherId).HasMaxLength(450);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FKEvent515872");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IssueType>(entity =>
            {
                entity.Property(e => e.Issue)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasKey(e => e.RepairId)
                    .HasName("PK__Maintena__07D0BC2D1A0082AB");

                entity.ToTable("Maintenance");

                entity.Property(e => e.DateCompleted).HasColumnType("date");

                entity.Property(e => e.DateOfRepair).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_VehicleMain");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.ToTable("RequestType");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("Station");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("TaskId");

                entity.ToTable("UserTask");

                entity.Property(e => e.LastDate).HasColumnType("date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.UserTasks)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FKTask776266");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle");

                entity.Property(e => e.VehicleId).HasColumnName("Vehicle_Id");

                entity.Property(e => e.LicenceExpiry).HasColumnType("date");

                entity.Property(e => e.LicencePlate)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Make)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.VehicleStatus)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.StationId)
                    .HasConstraintName("FK_Vehicle_2");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .HasConstraintName("FK_Vehicle_1");
            });

            modelBuilder.Entity<VehicleCatalogue>(entity =>
            {
                entity.HasKey(e => e.DefaultTypeId)
                    .HasName("PK__tmp_ms_x__08498062116F9B43");

                entity.ToTable("VehicleCatalogue");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MaximumGvr).HasColumnName("MaximumGVR");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypicalUse)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VehicleReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("ReportId");

                entity.Property(e => e.DateReported).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IssueType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VehicleReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ReportUser");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleReports)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_VehicleReport");
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.ToTable("VehicleType");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MaximumGvr).HasColumnName("MaximumGVR");

                entity.Property(e => e.TypicalUse)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
