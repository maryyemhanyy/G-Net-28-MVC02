using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
          
        public async Task<IEnumerable<MemberShip>> GetAllMembershipsWithMembersAndPlansAsync(Expression<Func<MemberShip, bool>>? predicate = null, CancellationToken ct = default)
        {

            IQueryable<MemberShip> query = _dbContext.MemberShips.AsNoTracking().Include(ms => ms.Plan).Include(ms => ms.Member);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync(ct);
        }

    }
}
