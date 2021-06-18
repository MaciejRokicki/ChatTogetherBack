﻿// <auto-generated />
using System;
using ChatTogether.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatTogether.Dal.Migrations
{
    [DbContext(typeof(ChatTogetherDbContext))]
    [Migration("20210617091630_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.AccountDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2021, 6, 17, 11, 16, 30, 603, DateTimeKind.Local).AddTicks(169));

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

                    b.HasKey("Id");

                    b.ToTable("Accounts");
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

            modelBuilder.Entity("ChatTogether.Dal.Dbos.Security.AccountDbo", b =>
                {
                    b.Navigation("ChangeEmailTokenDbo");

                    b.Navigation("ChangePasswordTokenDbo");

                    b.Navigation("ConfirmEmailTokenDbo");
                });
#pragma warning restore 612, 618
        }
    }
}
