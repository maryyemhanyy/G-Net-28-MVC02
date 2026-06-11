using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Models.Enums;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionServic : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServic(IUnitOfWork unitOfWork , IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End Date must be after Start Date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start Date must be in the future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer == null)  return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category == null) return Result.NotFound("Category Not Found");

            var checkSpeciality = Enum.TryParse<Specialties>(category.CategoryName,true, out var speciality);
            if(!checkSpeciality || trainer.Specialties!= speciality) return Result.Validation("Trainer Specialty does not match the Session Category");

            var session = _mapper.Map<Session>(model);


            var affectedRows = await _unitOfWork.GetRepository<Session>().AddAsync(session);

            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed to create session");

        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id, ct);

            if (session == null) return Result.NotFound("Session Not Found");

            if(session.EndDate >= DateTime.Now) return Result.Fail("Cannot delete a session that has not ended yet");

            var booked = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);

            if (booked > 0) return Result.Fail("Cannot delete a session that has booked slots");
  

            var affectedRows = await _unitOfWork.GetRepository<Session>().DeleteAsync(session);

            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed to delete session"); 
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync();

            if (!sessions.Any()) return null;

            var MappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - (await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct));
            }

            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForSelectAsync(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int id, CancellationToken ct = default)
        {
            var sesssion = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryAsync(id, ct);

            if (sesssion == null) return null;

            var MappedSession = _mapper.Map<SessionViewModel>(sesssion);

            MappedSession.AvailableSlots = MappedSession.Capacity - (await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(MappedSession.Id, ct));

            return MappedSession;
        }

        public async Task<UpdateSessionViewModel?> GetSessionForUpdateAsync(int id, CancellationToken ct = default)
        {
           var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);
         
           if (session == null) return null;

           if(!await SessionAvailabilityForUpdate(session, ct)) return null;    

           return _mapper.Map<UpdateSessionViewModel>(session);

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForSelectAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);

            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id, ct);

            if (session == null) return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now) return Result.Fail("Cannot edit an Ongoing or Completed session");

            var booked = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);

            if (booked > 0) return Result.Fail("Cannot update a session that has booked slots");

            if (model.EndDate <= model.StartDate) return Result.Validation("End Date must be after Start Date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start Date must be in the future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId, ct);
            if (category == null) return Result.NotFound("Category Not Found");

            var checkSpeciality = Enum.TryParse<Specialties>(category.CategoryName, true, out var speciality);
            if (!checkSpeciality || trainer.Specialties != speciality) return Result.Validation("Trainer Specialty does not match the Session Category");

            _mapper.Map(model, session);

            session.UpdatedAt = DateTime.Now;

            var affectedRows = await _unitOfWork.GetRepository<Session>().UpdateAsync(session);

            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed to update session");
        }

        #region Helper Method
        private async Task<bool> SessionAvailabilityForUpdate(Session session , CancellationToken ct)
        {
            if(session.StartDate < DateTime.UtcNow) return false;

            var bookedSlots = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);

            return bookedSlots == 0;
        }
        #endregion
    }
}
