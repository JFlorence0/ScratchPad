﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScratchPad.Models;

#nullable disable

namespace ScratchPad.Migrations
{
    [DbContext(typeof(ScratchPadDbContext))]
    [Migration("20250120145540_AddInvestmentIdeaModel")]
    partial class AddInvestmentIdeaModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ScratchPad.Models.InvestmentIdea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("InvestmentThemeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("InvestmentThemeId");

                    b.ToTable("InvestmentIdeas");
                });

            modelBuilder.Entity("ScratchPad.Models.InvestmentTheme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("InvestmentThemes");
                });

            modelBuilder.Entity("ScratchPad.Models.InvestmentIdea", b =>
                {
                    b.HasOne("ScratchPad.Models.InvestmentTheme", "InvestmentTheme")
                        .WithMany("InvestmentIdeas")
                        .HasForeignKey("InvestmentThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InvestmentTheme");
                });

            modelBuilder.Entity("ScratchPad.Models.InvestmentTheme", b =>
                {
                    b.Navigation("InvestmentIdeas");
                });
#pragma warning restore 612, 618
        }
    }
}
