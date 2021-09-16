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
    [Migration("20210916072124_t2")]
    partial class t2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("Chat.Code.Entities.Groups.CreateFriends", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("BeInvitedId")
                        .HasColumnType("char(36)");

                    b.Property<int>("CreateFriendsEnum")
                        .HasColumnType("int");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("InitiatorId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("BeInvitedId");

                    b.HasIndex("Id");

                    b.HasIndex("InitiatorId");

                    b.ToTable("CreateFriends");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.Friends", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("FriendId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Receiving")
                        .HasColumnType("longtext");

                    b.Property<Guid>("SelfId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.HasIndex("Id");

                    b.HasIndex("SelfId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.GroupData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Notice")
                        .HasColumnType("longtext");

                    b.Property<string>("Picture")
                        .HasColumnType("longtext");

                    b.Property<string>("PictureKey")
                        .HasColumnType("longtext");

                    b.Property<string>("Receiving")
                        .HasColumnType("longtext");

                    b.Property<Guid>("SelfId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("SelfId");

                    b.ToTable("GuroupData");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.GroupMembers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("GroupDataId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Receiving")
                        .HasColumnType("longtext");

                    b.Property<Guid>("SelfId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("GroupDataId");

                    b.HasIndex("Id");

                    b.HasIndex("SelfId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("Chat.Code.Entities.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

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

                    b.Property<string>("HeadPortraitKey")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("PassWord")
                        .HasColumnType("longtext");

                    b.Property<int>("Power")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UseState")
                        .HasColumnType("int");

                    b.Property<string>("UserNumber")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Chat.EntityFrameworkCore.Mappings.GroupsConfiguration.ChatMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FileName")
                        .HasColumnType("longtext");

                    b.Property<string>("HeadPortrait")
                        .HasColumnType("longtext");

                    b.Property<sbyte>("Marking")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Receiving")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("SendId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Receiving");

                    b.ToTable("ChatMessage");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.CreateFriends", b =>
                {
                    b.HasOne("Chat.Code.Entities.Users.User", "BeInvited")
                        .WithMany()
                        .HasForeignKey("BeInvitedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chat.Code.Entities.Users.User", "Initiator")
                        .WithMany()
                        .HasForeignKey("InitiatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BeInvited");

                    b.Navigation("Initiator");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.Friends", b =>
                {
                    b.HasOne("Chat.Code.Entities.Users.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chat.Code.Entities.Users.User", "Self")
                        .WithMany()
                        .HasForeignKey("SelfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("Self");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.GroupData", b =>
                {
                    b.HasOne("Chat.Code.Entities.Users.User", "Self")
                        .WithMany()
                        .HasForeignKey("SelfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Self");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.GroupMembers", b =>
                {
                    b.HasOne("Chat.Code.Entities.Groups.GroupData", "GroupData")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chat.Code.Entities.Users.User", "Self")
                        .WithMany()
                        .HasForeignKey("SelfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupData");

                    b.Navigation("Self");
                });

            modelBuilder.Entity("Chat.Code.Entities.Groups.GroupData", b =>
                {
                    b.Navigation("GroupMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
