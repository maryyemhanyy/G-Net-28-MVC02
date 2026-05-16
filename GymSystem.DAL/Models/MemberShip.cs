using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class MemberShip : BaseEntity
    {
        public DateTime EndDate { get; set; }

        [NotMapped]
        public string Status => EndDate > DateTime.UtcNow ? "Active" : "Expired"; 
        public int MemberId { get; set; }
        public Member Member { get; set; } = default!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = default!;


    }
}
