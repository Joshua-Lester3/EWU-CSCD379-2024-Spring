﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rhym.Api.Data;

#nullable disable

namespace Rhym.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240531174438_AddSyllable")]
    partial class AddSyllable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Rhym.Api.Models.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"));

                    b.Property<int>("DocumentDataId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("DocumentId");

                    b.HasIndex("DocumentDataId");

                    b.HasIndex("UserId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Rhym.Api.Models.DocumentData", b =>
                {
                    b.Property<int>("DocumentDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentDataId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentDataId");

                    b.ToTable("DocumentData");
                });

            modelBuilder.Entity("Rhym.Api.Models.Syllable", b =>
                {
                    b.Property<int>("SyllableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SyllableId"));

                    b.Property<string>("Syllables")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SyllableId");

                    b.ToTable("Syllables");
                });

            modelBuilder.Entity("Rhym.Api.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Rhym.Api.Models.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Phonemes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SyllableId")
                        .HasColumnType("int");

                    b.Property<string>("SyllablesPronunciation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WordKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SyllableId");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("Rhym.Api.Models.Document", b =>
                {
                    b.HasOne("Rhym.Api.Models.DocumentData", "DocumentData")
                        .WithMany()
                        .HasForeignKey("DocumentDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rhym.Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentData");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Rhym.Api.Models.Word", b =>
                {
                    b.HasOne("Rhym.Api.Models.Syllable", "Syllable")
                        .WithMany()
                        .HasForeignKey("SyllableId");

                    b.Navigation("Syllable");
                });
#pragma warning restore 612, 618
        }
    }
}
