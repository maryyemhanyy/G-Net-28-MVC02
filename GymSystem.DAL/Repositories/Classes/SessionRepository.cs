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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<Session> query = _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync(ct);
        }


        public Task<Session?> GetSessionWithTrainerAndCategoryAsync(int id, CancellationToken ct = default)
        => _dbContext.Sessions.AsNoTracking().Include(s => s.Category).Include(s => s.Trainer).FirstOrDefaultAsync(s => s.Id == id, ct);

        public Task<int> GetCountOfBookedSlotsAsync(int id, CancellationToken ct = default)
            =>_dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == id, ct);
           
        
    }
}
