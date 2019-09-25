﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheBitmexCollector;

namespace TheBitmexCollector.Migrations
{
    [DbContext(typeof(CollectorContext))]
    [Migration("20190627193304_TradeBins1")]
    partial class TradeBins1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity("TheBitmexCollector.Liquidation", b =>
                {
                    b.Property<int>("LiquidationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Direction");

                    b.Property<decimal>("Price");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("LiquidationId");

                    b.HasIndex("Symbol");

                    b.ToTable("Liquidations");
                });

            modelBuilder.Entity("TheBitmexCollector.TradeBin", b =>
                {
                    b.Property<int>("TradeBinId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Close");

                    b.Property<decimal>("ForeignNotional");

                    b.Property<decimal>("High");

                    b.Property<decimal>("HomeNotional");

                    b.Property<decimal>("Low");

                    b.Property<decimal>("Open");

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("Timestamp");

                    b.Property<decimal>("Trades");

                    b.Property<decimal>("Volume");

                    b.HasKey("TradeBinId");

                    b.HasIndex("Symbol");

                    b.HasIndex("Timestamp");

                    b.ToTable("TradeBins");
                });
#pragma warning restore 612, 618
        }
    }
}
