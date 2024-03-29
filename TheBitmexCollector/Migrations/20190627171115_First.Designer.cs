﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheBitmexCollector;

namespace TheBitmexCollector.Migrations
{
    [DbContext(typeof(CollectorContext))]
    [Migration("20190627171115_First")]
    partial class First
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

                    b.HasKey("LiquidationId");

                    b.ToTable("Liquidations");
                });
#pragma warning restore 612, 618
        }
    }
}
