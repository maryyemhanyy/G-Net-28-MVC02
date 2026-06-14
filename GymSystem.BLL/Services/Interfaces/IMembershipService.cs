using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);

        Task<Result> CreateMembershipAsync(CreateMembershipViewModel membership, CancellationToken ct = default);

        Task<IEnumerable<MemberSelectListViewModel>> GetMembersSelectListAsync(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlansSelectListAsync(CancellationToken ct = default);

        Task<Result> DeleteMembershipAsync(int id, CancellationToken ct = default);


    }
}
