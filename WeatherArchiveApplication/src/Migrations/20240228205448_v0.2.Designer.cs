﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeatherArchiveApp.Models;

#nullable disable

namespace WeatherArchiveApp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240228205448_v0.2")]
    partial class v02
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WeatherRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AtmosPressure")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("DateOfRecord")
                        .HasColumnType("date");

                    b.Property<float>("Dewpoint")
                        .HasColumnType("real");

                    b.Property<byte?>("HorizontalVisibility")
                        .HasColumnType("smallint");

                    b.Property<float>("Humidity")
                        .HasColumnType("real");

                    b.Property<byte?>("Overcast")
                        .HasColumnType("smallint");

                    b.Property<int?>("OvercastLowerLimit")
                        .HasColumnType("integer");

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.Property<TimeOnly>("TimeOfRecord")
                        .HasColumnType("time without time zone");

                    b.Property<string>("WeatherEvents")
                        .HasColumnType("text");

                    b.Property<string>("WindDirection")
                        .HasColumnType("text");

                    b.Property<byte?>("WindSpeed")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("WeatherRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
