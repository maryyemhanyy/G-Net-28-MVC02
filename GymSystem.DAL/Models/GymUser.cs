using GymSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Models
{
    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        [RegularExpression(@"(010|011|012|015)\d{8}$" , ErrorMessage = "ONLY Egyptian Phone Format")]
        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Address Address { get; set; } = default!;
    }

    public class Address
    {
        public int BuildingNumber { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

    }
}
