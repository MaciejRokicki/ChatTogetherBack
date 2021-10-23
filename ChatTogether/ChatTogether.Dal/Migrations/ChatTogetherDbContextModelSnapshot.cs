﻿// <auto-generated />
using System;
using ChatTogether.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatTogether.Dal.Migrations
{
    [DbContext(typeof(ChatTogetherDbContext))]
    partial class ChatTogetherDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChatTogether.Dal.Dbos.MessageDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReceivedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.MessageFileDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SourceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThumbnailName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageFiles");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.RoomDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MaxPeople")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.AccountDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BlockedAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("USER");

                    b.HasKey("Id");

                    b.HasIndex("BlockedAccountId")
                        .IsUnique()
                        .HasFilter("[BlockedAccountId] IS NOT NULL");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.BlockedAccountDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("BlockedTo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("BlockedAccounts");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.ChangeEmailTokenDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ChangeEmailTokens");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.ChangePasswordTokenDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ChangePasswordTokens");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.ConfirmEmailTokenDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ConfirmEmailTokens");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.UserDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nickname")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.MessageDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.RoomDbo", "Room")
                        .WithMany("Messages")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatTogether.Dal.Dbos.UserDbo", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.MessageFileDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.MessageDbo", "Message")
                        .WithMany("Files")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.AccountDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.Security.BlockedAccountDbo", "BlockedAccountDbo")
                        .WithOne("Account")
                        .HasForeignKey("ChatTogether.Dal.Dbos.Security.AccountDbo", "BlockedAccountId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("BlockedAccountDbo");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.BlockedAccountDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.Security.AccountDbo", "CreatedBy")
                        .WithMany("BlockedAccounts")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.ChangeEmailTokenDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.Security.AccountDbo", "Account")
                        .WithOne("ChangeEmailTokenDbo")
                        .HasForeignKey("ChatTogether.Dal.Dbos.Security.ChangeEmailTokenDbo", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.ChangePasswordTokenDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.Security.AccountDbo", "Account")
                        .WithOne("ChangePasswordTokenDbo")
                        .HasForeignKey("ChatTogether.Dal.Dbos.Security.ChangePasswordTokenDbo", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.ConfirmEmailTokenDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.Security.AccountDbo", "Account")
                        .WithOne("ConfirmEmailTokenDbo")
                        .HasForeignKey("ChatTogether.Dal.Dbos.Security.ConfirmEmailTokenDbo", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.UserDbo", b =>
                {
                    b.HasOne("ChatTogether.Dal.Dbos.Security.AccountDbo", "Account")
                        .WithOne("User")
                        .HasForeignKey("ChatTogether.Dal.Dbos.UserDbo", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.MessageDbo", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.RoomDbo", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.AccountDbo", b =>
                {
                    b.Navigation("BlockedAccounts");

                    b.Navigation("ChangeEmailTokenDbo");

                    b.Navigation("ChangePasswordTokenDbo");

                    b.Navigation("ConfirmEmailTokenDbo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.BlockedAccountDbo", b =>
                {
                    b.Navigation("Account");
                });

            modelBuilder.Entity("ChatTogether.Dal.Dbos.UserDbo", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
