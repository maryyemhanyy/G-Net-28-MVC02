using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.TrainerViewModels
{
    public class UpdateTrainerViewModel
    {
        public string Name { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(010|011|012|015)\d{8}$", ErrorMessage = "ONLY Egyptian Phone Format")]
        public string Phone { get; set; } = null!;


        [Required(ErrorMessage = "Building Number is required")]
        [Range(1, 9000, ErrorMessage = "Building Number must be more than 0")]
        public int BuildingNumber { get; set; }


        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "Street is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street can only contain letters and spaces")]
        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "Specialties is required.")]
        public Specialties Specialties { get; set; }


    }
}
