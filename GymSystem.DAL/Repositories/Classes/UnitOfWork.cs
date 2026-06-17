using GymSystem.DAL.DbContexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISessionRepository SessionRepository { get; }

        private readonly Dictionary<string,object> _repositories = [];
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext , ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var type = typeof(TEntity).Name;

            if(_repositories.TryGetValue(type, out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }

            var repository = new GenericRepository<TEntity>(_dbContext);
            _repositories[type] =  repository;
            return repository;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);

    }
}
