using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;

        public PlanService(IGenericRepository<Plan> planRepository, IGenericRepository<MemberShip> memberShipRepository)
        {
            _planRepository = planRepository;
            _memberShipRepository = memberShipRepository;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);

            if (!plans.Any()) return [];

            var planVM = plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive

            });

            return planVM;
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan == null) return null;

            var planVM = new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };

            return planVM;
        }

        public async Task<UpdatePlanViewModel?> GetPlanForEditAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan == null) return null;

            var updatePlanVM = new UpdatePlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };
            return updatePlanVM;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan == null) return false;

            if (plan.Name != model.Name) return false;

            var hasActiveMemberships = await _memberShipRepository.AnyAsync(m => m.PlanId == id && m.EndDate > DateTime.UtcNow, ct);

            if (hasActiveMemberships) return false;

            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;

            var result = await _planRepository.UpdateAsync(plan);

            return result > 0;
        }

        public async Task<bool> TogglePlanStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan == null) return false;

            if (plan.IsActive)
            {
                var hasActiveMemberships = await _memberShipRepository.AnyAsync(m => m.PlanId == id && m.EndDate > DateTime.UtcNow, ct);

                if (hasActiveMemberships) return false;
            }

            plan.IsActive = !plan.IsActive;

            var result = await _planRepository.UpdateAsync(plan);

            return result > 0;
        }
    }
}
