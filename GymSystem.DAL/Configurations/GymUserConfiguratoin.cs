using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Configurations
{
    public class GymUserConfiguratoin<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name).HasColumnType("varchar").HasMaxLength(50);

            builder.Property(u => u.Email).HasColumnType("varchar").HasMaxLength(100);

            builder.Property(u => u.Phone).HasColumnType("varchar").HasMaxLength(11);

            builder.OwnsOne(u => u.Address, Address =>
            {
                Address.Property(a => a.City).HasColumnType("varchar").HasMaxLength(30);

                Address.Property(a => a.Street).HasColumnType("varchar").HasMaxLength(30);

            });

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasIndex(u => u.Phone).IsUnique();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("GymUser_EmailCheck" , "Email LIKE '_%@_%._%'");

                t.HasCheckConstraint("GymUser_PhoneCheck", "[Phone] LIKE '010%' OR [Phone] LIKE '011%' OR [Phone] LIKE '012%' OR [Phone] LIKE '015%'");
            });
        }
    }
}
