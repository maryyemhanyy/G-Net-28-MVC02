using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is required ")]
        [Range(0.1 , 300 , ErrorMessage = "Height must be greater than 0")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight is required ")]
        [Range(0.1, 500, ErrorMessage = "Weight must be greater than 0")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type is required ")]
        [StringLength(3 , ErrorMessage = "Blood Type must be at most 3 characters")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
