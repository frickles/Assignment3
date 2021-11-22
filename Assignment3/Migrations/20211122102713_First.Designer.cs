﻿// <auto-generated />
using System;
using Assignment3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment3.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211122102713_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Assignment3.Cinema", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("ScreeningID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("ScreeningID");

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("Assignment3.Movie", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PosterPath")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<short>("Runtime")
                        .HasColumnType("smallint");

                    b.Property<int?>("ScreeningID")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("ScreeningID");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Assignment3.Screening", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("TicketID")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time(0)");

                    b.HasKey("ID");

                    b.HasIndex("TicketID");

                    b.ToTable("Screenings");
                });

            modelBuilder.Entity("Assignment3.Ticket", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("TimePurchased")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Assignment3.Cinema", b =>
                {
                    b.HasOne("Assignment3.Screening", null)
                        .WithMany("Cinemas")
                        .HasForeignKey("ScreeningID");
                });

            modelBuilder.Entity("Assignment3.Movie", b =>
                {
                    b.HasOne("Assignment3.Screening", null)
                        .WithMany("Movies")
                        .HasForeignKey("ScreeningID");
                });

            modelBuilder.Entity("Assignment3.Screening", b =>
                {
                    b.HasOne("Assignment3.Ticket", null)
                        .WithMany("Screenings")
                        .HasForeignKey("TicketID");
                });

            modelBuilder.Entity("Assignment3.Screening", b =>
                {
                    b.Navigation("Cinemas");

                    b.Navigation("Movies");
                });

            modelBuilder.Entity("Assignment3.Ticket", b =>
                {
                    b.Navigation("Screenings");
                });
#pragma warning restore 612, 618
        }
    }
}
