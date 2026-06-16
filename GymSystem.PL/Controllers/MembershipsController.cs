using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.PL.Controllers
{
    //[Authorize]
    public class MembershipsController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var memberships = await _membershipService.GetAllMembershipsAsync(ct);
            return View(memberships);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await DropDownList(ct);
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await DropDownList(ct);
                return View(model);
            }
            var result = await _membershipService.CreateMembershipAsync(model, ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while creating the membership.";
                await DropDownList(ct);
                return View(model);
            }
        }
        #endregion

        #region Delete

        [HttpPost]

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _membershipService.DeleteMembershipAsync(id, ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Membership canceled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "An error occurred while canceling the membership.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


        private async Task DropDownList(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await _membershipService.GetPlansSelectListAsync(ct), "Id", "Name");

            ViewBag.Members = new SelectList(await _membershipService.GetMembersSelectListAsync(ct), "Id", "Name");
        }
    }
}
