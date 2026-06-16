using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }
        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {

            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _trainerService.CreateTrainerAsync(model, ct);

            if (result)
            {

                TempData["SuccessMessage"] = "Trainer created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create trainer. Please try again.";
            }
            return RedirectToAction("Index");

        }

        #endregion

        #region Details

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetails(id, ct);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(trainer);
            }
        }
        #endregion

        #region Edit
        [HttpGet]

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {

            var trainer = await _trainerService.GetTrainerForEditAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(trainer);

            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Edit), model);

            var result = await _trainerService.UpdateTrainerAsync(id, model, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update trainer. Please try again.";
                return View(nameof(Edit), model);
            }

        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {

            var trainer = await _trainerService.GetTrainerDetails(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerService.RemoveTrainerAsync(id, ct);

            if(result)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete trainer. Please try again.";
                return RedirectToAction(nameof(Index));

            }
        }
        #endregion
    }
}