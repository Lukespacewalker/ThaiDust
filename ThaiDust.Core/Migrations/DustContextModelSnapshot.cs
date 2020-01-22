﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Migrations
{
    [DbContext(typeof(DustContext))]
    partial class DustContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("ThaiDust.Core.Model.Persistent.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Param")
                        .HasColumnType("TEXT");

                    b.Property<string>("StationCode")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("StationCode");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("ThaiDust.Core.Model.Persistent.Station", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Code");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("ThaiDust.Core.Model.Persistent.Record", b =>
                {
                    b.HasOne("ThaiDust.Core.Model.Persistent.Station", null)
                        .WithMany("Records")
                        .HasForeignKey("StationCode");
                });
#pragma warning restore 612, 618
        }
    }
}
