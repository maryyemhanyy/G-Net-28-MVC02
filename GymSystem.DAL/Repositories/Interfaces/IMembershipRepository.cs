using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<MemberShip>
    {
        public Task<IEnumerable<MemberShip>> GetAllMembershipsWithMembersAndPlansAsync(Expression<Func<MemberShip, bool>>? predicate = null, CancellationToken ct = default);


    }
}
