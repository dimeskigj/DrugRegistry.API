﻿// <auto-generated />
using System;
using DrugRegistry.API.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DrugRegistry.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230228205246_ChangePriceFromIntToDouble")]
    partial class ChangePriceFromIntToDouble
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("DecisionDate")
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
#pragma warning restore 612, 618
        }
    }
}
