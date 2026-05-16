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
    public class MemberShipConfiguratoin : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Ignore(m => m.Id);

            builder.HasKey(m => new { m.MemberId, m.PlanId });

            builder.Property(m => m.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");
        }
    }
}
