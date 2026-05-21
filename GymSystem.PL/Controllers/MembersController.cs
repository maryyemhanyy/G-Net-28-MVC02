using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService) {
            _memberService = memberService;
        }

        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Members = await _memberService.GetAllMembersAsync(ct);
            return View(Members);
        }
        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult>CreateMember(CreateMemberViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _memberService.CreateMemberAsync(model, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create member";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details

        public async Task<IActionResult> MemberDetails(int Id, CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberDetails(Id, ct);

            if(result is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }
        #endregion
    }
}
