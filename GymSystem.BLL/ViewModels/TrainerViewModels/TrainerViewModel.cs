using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.TrainerViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Specialization { get; set; } = null!;

        //Trainer Details
         public string DateOfBirth { get; set; } = null!;

         public int BuildingNumber { get; set; }

         public string City { get; set; } = null!;
         public string Street { get; set; } = null!;

    }
}
