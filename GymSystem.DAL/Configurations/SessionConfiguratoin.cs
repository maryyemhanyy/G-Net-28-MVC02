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
    public class SessionConfiguratoin : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("SessionCapacityCheck", "Capacity BETWEEN 1 AND 25");

                t.HasCheckConstraint("EndDateAfterStartDate", "EndDate>StartDate");
            });


        }
    }
}
