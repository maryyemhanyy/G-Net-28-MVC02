using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.AttachmentServices;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper , IAttachmentService attachmentService) {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();

            if(await memberRepo.AnyAsync(m => m.Email == member.Email, ct)) 
                return Result.Fail("Email already exists");

            if (await memberRepo.AnyAsync(m => m.Phone == member.Phone, ct)) 
                return Result.Fail("Phone number already exists");

            var photo = await _attachmentService.UploadAttachmentAsync(member.Photo.OpenReadStream(),member.Photo.FileName, "MembersPictures", ct);

            if (string.IsNullOrEmpty(photo)) return Result.Validation("Photo upload failed");

            var Member = _mapper.Map<Member>(member);
            Member.Photo = photo;        

            var result = await memberRepo.AddAsync(Member);

            if (result == 0) 
            {
               if(!string.IsNullOrEmpty(Member.Photo))
                {
                    _attachmentService.DeleteAttachment(Member.Photo, "Members");
                }

                return Result.Fail("Failed to create member");
            }

            return Result.Ok();

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();
            var members = await memberRepo.GetAllAsync(ct:ct);

            if (!members.Any()) return [];

            var memberVM = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return memberVM;
        }

        public async Task<MemberViewModel?> GetMemberDetails(int Id, CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();
            var member = await memberRepo.GetByIdAsync(Id, ct);

            if (member == null) return null;

            var memberVM = _mapper.Map<MemberViewModel>(member);

            var ActiveMemberShip = await _unitOfWork.MembershipRepository.FirstOrDefaultAsync(m => m.MemberId == Id && m.EndDate > DateTime.UtcNow , ct:ct);

            if(ActiveMemberShip is not null)
            {
                var ActivePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(ActiveMemberShip.PlanId , ct);
                memberVM.PlanName = ActivePlan?.Name;
                memberVM.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                memberVM.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }
            return memberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int MemberId, CancellationToken ct = default)
        {
            var healthRecord =await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(h => h.MemberId == MemberId, ct: ct);

            if (healthRecord is null) return null;
            else
            {

                return _mapper.Map<HealthRecordViewModel>(healthRecord);
            }
        }

        public async Task<Result> RemoveMemberAsync(int MemberId, CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();

            var Member = await memberRepo.GetByIdAsync(MemberId, ct: ct);

            if (Member is null) return Result.NotFound("Member not found");

            var HasFutureSession =await _unitOfWork.GetRepository<Booking>().AnyAsync(x=>x.MemberId==MemberId && x.Session.StartDate > DateTime.UtcNow, ct: ct);

            if(HasFutureSession) return Result.Fail("Member has future sessions, cannot delete");     

            var result = await memberRepo.DeleteAsync(Member);

            if (result > 0)
            {
                if(!string.IsNullOrEmpty(Member.Photo))
                {
                    _attachmentService.DeleteAttachment(Member.Photo, "Members");
                }
                return Result.Ok();

            }
            return Result.Fail("Failed to delete member");
        }

        public async Task<UpdateMemberViewModel?>MemberToUpdateAsync(int MemberId, CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();
            var member = await memberRepo.GetByIdAsync(MemberId, ct: ct);

            if (member is null) return null;


                return _mapper.Map<UpdateMemberViewModel>(member);
            }

        public async Task<Result> UpdateMemberDetailsAsync(int MemberId, UpdateMemberViewModel member, CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();
            var Member = await memberRepo.GetByIdAsync(MemberId, ct: ct);

            if (Member is null) return Result.NotFound("Member not found");

            if(await memberRepo.AnyAsync(m=>m.Email == member.Email && m.Id!=MemberId))
                return Result.Fail("Email already exists");
            if (await memberRepo.AnyAsync(m => m.Phone == member.Phone && m.Id != MemberId)) 
                return Result.Fail("Phone number already exists");

            _mapper.Map(member, Member);
           
            var result = await memberRepo.UpdateAsync(Member);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update member details");

        }
    }
}
