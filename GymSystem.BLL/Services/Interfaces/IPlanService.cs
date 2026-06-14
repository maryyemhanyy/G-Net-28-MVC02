using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);

        Task<PlanViewModel?> GetPlanDetailsAsync(int id , CancellationToken ct = default);

        Task<UpdatePlanViewModel?> GetPlanForEditAsync(int id , CancellationToken ct = default);

        Task<bool>UpdatePlanAsync(int id , UpdatePlanViewModel plan , CancellationToken ct = default);

        Task<bool> TogglePlanStatusAsync(int id, CancellationToken ct = default);
    }
}
