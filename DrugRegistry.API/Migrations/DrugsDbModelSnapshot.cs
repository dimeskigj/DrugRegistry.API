﻿// <auto-generated />
using System;
using DrugRegistry.API.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DrugRegistry.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class DrugsDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DrugRegistry.API.Domain.Drug", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ApprovalCarrier")
                        .HasColumnType("text");

                    b.Property<string>("Atc")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DecisionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DecisionNumber")
                        .HasColumnType("text");

                    b.Property<string>("GenericName")
                        .HasColumnType("text");

                    b.Property<string>("Ingredients")
                        .HasColumnType("text");

                    b.Property<int>("IssuingType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LatinName")
                        .HasColumnType("text");

                    b.Property<string>("ManualUrl")
                        .HasColumnType("text");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("text");

                    b.Property<string>("Packaging")
                        .HasColumnType("text");

                    b.Property<string>("PharmaceuticalForm")
                        .HasColumnType("text");

                    b.Property<double>("PriceWithVat")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceWithoutVat")
                        .HasColumnType("double precision");

                    b.Property<string>("ReportUrl")
                        .HasColumnType("text");

                    b.Property<string>("Strength")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ValidityDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Drugs");
                });

            modelBuilder.Entity("DrugRegistry.API.Domain.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("DrugRegistry.API.Domain.Pharmacy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool?>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<bool?>("Central")
                        .HasColumnType("boolean");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Decision")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("IdNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Municipality")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Pharmacists")
                        .HasColumnType("text");

                    b.Property<int?>("PharmacyType")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Place")
                        .HasColumnType("text");

                    b.Property<string>("TaxNumber")
                        .HasColumnType("text");

                    b.Property<string>("Technicians")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Pharmacies");
                });

            modelBuilder.Entity("DrugRegistry.API.Domain.Pharmacy", b =>
                {
                    b.HasOne("DrugRegistry.API.Domain.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.Navigation("Location");
                });
#pragma warning restore 612, 618
        }
    }
}
