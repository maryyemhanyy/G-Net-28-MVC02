using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default);

        Task<SessionViewModel?> GetSessionByIdAsync(int id, CancellationToken ct = default);

        Task<UpdateSessionViewModel?>GetSessionForUpdateAsync(int id, CancellationToken ct = default);

        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct);

        Task<Result>CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct);

        Task<Result>DeleteSessionAsync(int id, CancellationToken ct);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForSelectAsync(CancellationToken ct = default);

        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForSelectAsync(CancellationToken ct = default);
    }
}
