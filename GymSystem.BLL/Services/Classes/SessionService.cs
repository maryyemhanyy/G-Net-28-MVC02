using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionService(IUnitOfWork unitOfWork) : ISessionService
    {
        public Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteSessionAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForSelectAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<SessionViewModel?> GetSessionByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateSessionViewModel> GetSessionForUpdateAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForSelectAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
