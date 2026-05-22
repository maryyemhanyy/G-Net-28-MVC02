using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<HealthRecord> _HealthRecordRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;

        public MemberService(IGenericRepository<Member> MemberRepository , IGenericRepository<MemberShip> MemberShipRepository,
             IGenericRepository<Plan> PlanRepository, IGenericRepository<HealthRecord> HealthRecordRepository, IGenericRepository<Booking> BookingRepository) {
            _memberRepository = MemberRepository;
            _memberShipRepository = MemberShipRepository;
            _planRepository = PlanRepository;
            _HealthRecordRepository = HealthRecordRepository;
            _bookingRepository = BookingRepository;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var EmailExist = await _memberRepository.AnyAsync(x => x.Email == member.Email);
            var PhoneExist = await _memberRepository.AnyAsync(x => x.Phone == member.Phone);

            if(EmailExist || PhoneExist) return false;

            var Member = new Member()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = member.BuildingNumber,
                    City = member.City,
                    Street = member.Street
                },
                HealthRecord = new HealthRecord()
                {
                    Height = member.HealthRecordViewModel.Height,
                    Weight = member.HealthRecordViewModel.Weight,
                    BloodType = member.HealthRecordViewModel.BloodType,
                    Note = member.HealthRecordViewModel.Note
                }
                
            };

            var result = await _memberRepository.AddAsync(Member);
            return result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct:ct);

            if (!members.Any()) return [];

            var memberVM = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            });

            return memberVM;
        }

        public async Task<MemberViewModel?> GetMemberDetails(int Id, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(Id, ct);

            if (member == null) return null;

            var memberVM = new MemberViewModel
            {
                Name = member.Name,
                Email =member.Email,
                Phone =member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.City} - {member.Address.Street}"
            };

            var ActiveMemberShip = await _memberShipRepository.FirstOrDefaultAsync(m => m.MemberId == Id && m.EndDate > DateTime.UtcNow , ct:ct);

            if(ActiveMemberShip is not null)
            {
                var ActivePlan = await _planRepository.GetByIdAsync(ActiveMemberShip.PlanId , ct);
                memberVM.PlanName = ActivePlan?.Name;
                memberVM.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                memberVM.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }
            return memberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int MemberId, CancellationToken ct = default)
        {
            var healthRecord =await _HealthRecordRepository.FirstOrDefaultAsync(h => h.MemberId == MemberId, ct: ct);

            if (healthRecord is null) return null;
            else
            {
                return new HealthRecordViewModel()
                {
                    Weight = healthRecord.Weight,
                    Height = healthRecord.Height,
                    BloodType = healthRecord.BloodType,
                    Note = healthRecord.Note               
                };
            }
        }

        public async Task<bool> RemoveMemberAsync(int MemberId, CancellationToken ct = default)
        {
            var Member = await _memberRepository.GetByIdAsync(MemberId, ct: ct);

            if (Member is null) return false;

            var HasFutureSession =await _bookingRepository.AnyAsync(x=>x.MemberId==MemberId && x.Session.StartDate > DateTime.UtcNow, ct: ct);

            if(HasFutureSession) return false;

            var result = await _memberRepository.DeleteAsync(Member);

            return result > 0;

        }

        public async Task<UpdateMemberViewModel?>MemberToUpdateAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(MemberId, ct: ct);

            if (member is null) return null;

            else
            {
                return new UpdateMemberViewModel
                {
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Photo = member.Photo,
                    BuildingNumber = member.Address.BuildingNumber,
                    City = member.Address.City,
                    Street = member.Address.Street
                };
            }
        }

        public async Task<bool> UpdateMemberDetailsAsync(int MemberId, UpdateMemberViewModel member, CancellationToken ct = default)
        {
            var Member = await _memberRepository.GetByIdAsync(MemberId, ct: ct);

            if (Member is null) return false;

            if(await _memberRepository.AnyAsync(m=>m.Email == member.Email && m.Id!=MemberId))  return false;
            if (await _memberRepository.AnyAsync(m => m.Phone == member.Phone && m.Id != MemberId)) return false;

            Member.Email = member.Email;
            Member.Phone = member.Phone;
            Member.Photo = member.Photo;
            Member.Address.BuildingNumber = member.BuildingNumber;
            Member.Address.City = member.City;
            Member.Address.Street = member.Street;
            Member.UpdatedAt = DateTime.UtcNow;

            var result = await _memberRepository.UpdateAsync(Member);

            return result > 0 ? true : false;

        }
    }
}
