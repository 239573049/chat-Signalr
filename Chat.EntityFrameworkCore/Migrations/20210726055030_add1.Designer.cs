﻿// <auto-generated />
using System;
using Chat.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chat.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(MasterDbContext))]
    [Migration("20210726055030_add1")]
    partial class add1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("Chat.Code.Entities.User.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Freezetime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HeadPortrait")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("PassWrod")
                        .HasColumnType("longtext");

                    b.Property<int>("Power")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });
#pragma warning restore 612, 618
        }
    }
}
