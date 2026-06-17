using GymSystem.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);

        Task<TrainerViewModel?> GetTrainerDetails(int TrainerId , CancellationToken ct = default);

        Task<bool> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default);

        Task<UpdateTrainerViewModel?>GetTrainerForEditAsync(int TrainerId , CancellationToken ct = default);

        Task<bool> UpdateTrainerAsync(int TrainerId , UpdateTrainerViewModel trainer , CancellationToken ct = default);

        Task<bool> RemoveTrainerAsync(int TrainerId , CancellationToken ct = default);
    }
}
