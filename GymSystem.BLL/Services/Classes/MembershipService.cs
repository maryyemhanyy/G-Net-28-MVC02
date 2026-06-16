using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships =await _unitOfWork.MembershipRepository.GetAllMembershipsWithMembersAndPlansAsync(ms => ms.Status == "Active", ct);
            return _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
        }

        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel membership, CancellationToken ct = default)
        {
            var memberExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == membership.MemberId, ct);
            if (!memberExists)
                return Result.NotFound("Member not found");

            var planExists = await _unitOfWork.GetRepository<Plan>().AnyAsync(p => p.Id == membership.PlanId, ct);
            if (!planExists)
                return Result.NotFound("Plan not found");

            var hasActiveMembership = await _unitOfWork.MembershipRepository.AnyAsync(ms => ms.MemberId == membership.MemberId && ms.EndDate > DateTime.Now, ct);

            if (hasActiveMembership)
                return Result.Fail("Member already has an active membership");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(membership.PlanId, ct);
            if (!plan.IsActive)
                return Result.Fail("Plan is not active");

            var membershipEntity = _mapper.Map<MemberShip>(membership);
            membershipEntity.EndDate = (membership.StartDate ?? DateTime.Now).AddDays(plan.DurationDays);

            await _unitOfWork.MembershipRepository.AddAsync(membershipEntity);

            var result =await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to create membership");


        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersSelectListAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansSelectListAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }

        public async Task<Result> DeleteMembershipAsync(int id, CancellationToken ct = default)

        {
            var membership =await _unitOfWork.MembershipRepository.FirstOrDefaultAsync(ms => ms.Id == id && ms.EndDate > DateTime.Now , true);

            if (membership == null) return Result.Fail("Membership not found" , ResultType.NotFound);

             await _unitOfWork.MembershipRepository.DeleteAsync(membership);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to delete membership");
        }

       
    }
}
