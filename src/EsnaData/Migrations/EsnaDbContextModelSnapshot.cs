﻿// <auto-generated />
using System;
using EsnaData.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EsnaData.Migrations
{
    [DbContext(typeof(EsnaDbContext))]
    partial class EsnaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("EsnaData.Entities.Command", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Data")
                        .HasColumnType("BLOB");

                    b.Property<long>("DeviceId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Executed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ExecutedOnUtc")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Function")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("Command");
                });

            modelBuilder.Entity("EsnaData.Entities.Configuration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaudRate")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<int>("DataBits")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Parity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PortName")
                        .HasColumnType("TEXT");

                    b.Property<int>("StopBits")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Timeout")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("EsnaData.Entities.Device", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExteraInfornamtion")
                        .HasColumnType("TEXT");

                    b.Property<byte>("FirstRegister")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MacAddress")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Offset")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("UnitId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("EsnaData.Entities.Recorde", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ConfigurationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Data")
                        .HasColumnType("BLOB");

                    b.Property<long>("DeviceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.HasIndex("DeviceId");

                    b.ToTable("Recorde");
                });

            modelBuilder.Entity("EsnaData.Entities.Command", b =>
                {
                    b.HasOne("EsnaData.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EsnaData.Entities.Recorde", b =>
                {
                    b.HasOne("EsnaData.Entities.Configuration", null)
                        .WithMany("Recordes")
                        .HasForeignKey("ConfigurationId");

                    b.HasOne("EsnaData.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
