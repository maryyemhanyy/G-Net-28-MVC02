using GymSystem.BLL.ViewModels.AnalyticsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        public Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct);
    }
}
