using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo {  get; set; }

        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

        public ICollection<MemberShip> MemberShips { get; set; } = new HashSet<MemberShip>();

    }
}
