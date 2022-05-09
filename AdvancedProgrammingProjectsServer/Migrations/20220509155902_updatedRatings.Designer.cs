﻿// <auto-generated />
using System;
using AdvancedProgrammingProjectsServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AdvancedProgrammingProjectsServer.Migrations
{
    [DbContext(typeof(AdvancedProgrammingProjectsServerContext))]
    [Migration("20220509155902_updatedRatings")]
    partial class updatedRatings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Contact", b =>
                {
                    b.Property<string>("contactOf")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("name")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RegisteredUserusername")
                        .HasColumnType("varchar(255)");

                    b.HasKey("contactOf", "name");

                    b.HasIndex("RegisteredUserusername");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RegisteredUserusername")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("with")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("RegisteredUserusername");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Message", b =>
                {
                    b.Property<int>("key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ConversationId")
                        .HasColumnType("int");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("sender")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("key");

                    b.HasIndex("ConversationId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.PendingUser", b =>
                {
                    b.Property<string>("username")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("encryptionAlgorithm")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nickname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("salt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("secretQuestionsId")
                        .HasColumnType("int");

                    b.Property<string>("verificationcode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("username");

                    b.HasIndex("secretQuestionsId");

                    b.ToTable("PendingUser");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Feedback")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeSubmitted")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.RegisteredUser", b =>
                {
                    b.Property<string>("username")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("encryptionAlgorithm")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nickNum")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nickname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("salt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("secretQuestionsId")
                        .HasColumnType("int");

                    b.Property<string>("verificationcode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("username");

                    b.HasIndex("secretQuestionsId");

                    b.ToTable("RegisteredUser");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.SecretQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("SecretQuestion");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Contact", b =>
                {
                    b.HasOne("AdvancedProgrammingProjectsServer.Models.RegisteredUser", null)
                        .WithMany("contacts")
                        .HasForeignKey("RegisteredUserusername");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Conversation", b =>
                {
                    b.HasOne("AdvancedProgrammingProjectsServer.Models.RegisteredUser", null)
                        .WithMany("conversations")
                        .HasForeignKey("RegisteredUserusername");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Message", b =>
                {
                    b.HasOne("AdvancedProgrammingProjectsServer.Models.Conversation", null)
                        .WithMany("messages")
                        .HasForeignKey("ConversationId");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.PendingUser", b =>
                {
                    b.HasOne("AdvancedProgrammingProjectsServer.Models.SecretQuestion", "secretQuestions")
                        .WithMany()
                        .HasForeignKey("secretQuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("secretQuestions");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.RegisteredUser", b =>
                {
                    b.HasOne("AdvancedProgrammingProjectsServer.Models.SecretQuestion", "secretQuestions")
                        .WithMany()
                        .HasForeignKey("secretQuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("secretQuestions");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.Conversation", b =>
                {
                    b.Navigation("messages");
                });

            modelBuilder.Entity("AdvancedProgrammingProjectsServer.Models.RegisteredUser", b =>
                {
                    b.Navigation("contacts");

                    b.Navigation("conversations");
                });
#pragma warning restore 612, 618
        }
    }
}