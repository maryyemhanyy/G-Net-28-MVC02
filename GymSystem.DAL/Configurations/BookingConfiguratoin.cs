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
    public class BookingConfiguratoin : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);

            builder.HasKey(b => new { b.SessionId, b.MemberId });

            builder.Property(b => b.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");

        }
    }
}
