using AutoMapper;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            MapSession();
            MapMember();
            MapMembership();
        }

        #region Session
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                                                   .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                                                   .ForMember(dest => dest.AvailableSlots , opt => opt.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<UpdateSessionViewModel , Session>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Category , CategorySelectViewModel>();
        }
        #endregion

        #region Member

        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>().ForMember(dest => dest.PlanName, opt => opt.Ignore())
                                                .ForMember(dest => dest.MemberShipStartDate, opt => opt.Ignore())
                                                .ForMember(dest => dest.MemberShipEndDate, opt => opt.Ignore());

            CreateMap<CreateMemberViewModel, Member>().ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address{
                                                                                                               BuildingNumber = src.BuildingNumber,
                                                                                                               City = src.City,
                                                                                                               Street = src.Street
                                                                                                           }))
                                                      .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));

            CreateMap<UpdateMemberViewModel, Member>().ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                BuildingNumber = src.BuildingNumber,
                City = src.City,
                Street = src.Street
            }));

            CreateMap<Member, UpdateMemberViewModel>().ForMember(dest => dest.BuildingNumber,opt => opt.MapFrom(src => src.Address.BuildingNumber))
                                                      .ForMember(dest => dest.City,opt => opt.MapFrom(src => src.Address.City))
                                                      .ForMember(dest => dest.Street,opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

        }
        #endregion

        #region Membership
        private void MapMembership()
        {
            CreateMap<MemberShip, MembershipViewModel>().ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                                                         .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                                                         .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));


            CreateMap<CreateMembershipViewModel, MemberShip>();

            CreateMap<Member, MemberSelectListViewModel>();

            CreateMap<Plan, PlanSelectListViewModel>();


        }
        #endregion

    }
}
