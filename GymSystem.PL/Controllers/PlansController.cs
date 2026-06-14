using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }
        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planService.GetAllPlansAsync(ct);

            return View(plans);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int Id, CancellationToken ct = default)
        {
            var plan = await _planService.GetPlanDetailsAsync(Id, ct);

            if (plan == null) return RedirectToAction(nameof(Index));

            return View(plan);
        }
        #endregion

        #region Edit
        [HttpGet]

        public async Task<IActionResult> Edit(int Id, CancellationToken ct = default)
        {
            var plan = await _planService.GetPlanForEditAsync(Id, ct);

            if (plan == null) return RedirectToAction(nameof(Index));

            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return View(model);

            var updatePlan = await _planService.UpdatePlanAsync(Id, model, ct);

            if (!updatePlan) {

            TempData["ErrorMessage"] ="Failed to update the plan. Please try again.";

            return View(model);
            }
            else
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
                return RedirectToAction(nameof(Index));
            }
               

        }
        #endregion

        #region Activation

        public async Task<IActionResult> Activation(int id, CancellationToken ct)
        {
            var result = await _planService.TogglePlanStatusAsync(id, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Plan status chaned";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "This plan has a membership!";
                return RedirectToAction(nameof(Index));
            }
            #endregion

        }
    }
}
