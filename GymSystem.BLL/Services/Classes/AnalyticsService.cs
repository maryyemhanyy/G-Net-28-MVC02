using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.AnalyticsViewModel;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            var sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(ct: ct);
            var memberships = await _unitOfWork.GetRepository<MemberShip>().GetAllAsync(ct: ct);


            return new AnalyticsViewModel
            {
                TotalMembers = members.Count(),
                ActiveMembers = memberships.Where(ms => ms.EndDate > DateTime.Now).Select(ms => ms.MemberId).Distinct().Count(),
                TotalTrainers = trainers.Count(),
                UpcomingSessions = sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = sessions.Count(s => s.EndDate < DateTime.Now)
            };

        }
    }
}
