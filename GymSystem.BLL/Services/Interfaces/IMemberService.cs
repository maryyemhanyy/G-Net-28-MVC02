using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);

        Task<MemberViewModel?> GetMemberDetails(int MemberId , CancellationToken ct = default);

        Task<Result> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);

        Task<UpdateMemberViewModel?>MemberToUpdateAsync(int MemberId , CancellationToken ct = default);

        Task<Result> UpdateMemberDetailsAsync(int MemberId , UpdateMemberViewModel member , CancellationToken ct = default);

        Task<HealthRecordViewModel?>GetMemberHealthRecordAsync(int MemberId , CancellationToken ct = default);

        Task<Result>RemoveMemberAsync(int MemberId , CancellationToken ct = default);
    }
}
