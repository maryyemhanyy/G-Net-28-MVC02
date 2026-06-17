using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepo;
        private readonly IGenericRepository<Session> _sessionRepo;

        public TrainerService(IGenericRepository<Trainer> trainerRepo , IGenericRepository<Session> sessionRepo)
        {
            _trainerRepo = trainerRepo;
            _sessionRepo = sessionRepo;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _trainerRepo.GetAllAsync(ct: ct);

            if (trainers is null) return Enumerable.Empty<TrainerViewModel>();

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                DateOfBirth = t.DateOfBirth.ToShortDateString(),
                Specialization = t.Specialties.ToString(),
            });
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default)
        {
            var emailExist = await _trainerRepo.AnyAsync(t => t.Email == trainer.Email, ct);
            var phoneExist = await _trainerRepo.AnyAsync(t => t.Phone == trainer.Phone, ct);

            if (emailExist || phoneExist) return false;

            var newTrainer = (new Trainer()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                Specialties = trainer.Specialties,
                Address = new Address()
                {
                    BuildingNumber = trainer.BuildingNumber,
                    City = trainer.City,
                    Street = trainer.Street
                },
                Gender = trainer.Gender
            });

            var result = await _trainerRepo.AddAsync(newTrainer);
            return result > 0;

        }



        public async Task<TrainerViewModel?> GetTrainerDetails(int id, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(id, ct);

            if (trainer is null) return null;

            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Specialization = trainer.Specialties.ToString(),
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street

            };
        }

        public async Task<UpdateTrainerViewModel?> GetTrainerForEditAsync(int TrainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(TrainerId, ct);

            if (trainer is null) return null;

            return new UpdateTrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                Specialties = trainer.Specialties,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                Gender = trainer.Gender
            };
        }

        public async Task<bool> UpdateTrainerAsync(int TrainerId, UpdateTrainerViewModel trainer, CancellationToken ct = default)
        {
            var existingTrainer = await _trainerRepo.GetByIdAsync(TrainerId, ct);

            if (existingTrainer is null) return false;

            if (await _trainerRepo.AnyAsync(t => t.Email == trainer.Email && t.Id != TrainerId, ct)) return false;
            if (await _trainerRepo.AnyAsync(t => t.Phone == trainer.Phone && t.Id != TrainerId, ct)) return false;

            if (existingTrainer.Name != trainer.Name) return false;


            existingTrainer.Email = trainer.Email;
            existingTrainer.Phone = trainer.Phone;
            existingTrainer.Specialties = trainer.Specialties;
            existingTrainer.Address.BuildingNumber = trainer.BuildingNumber;
            existingTrainer.Address.City = trainer.City;
            existingTrainer.Address.Street = trainer.Street;

            var result = await _trainerRepo.UpdateAsync(existingTrainer);

            return result > 0;
        }

        public async Task<bool> RemoveTrainerAsync(int TrainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(TrainerId, ct);

            if (trainer == null) return false;

            var hasActiveSessions = await _sessionRepo.AnyAsync(s => s.TrainerId == TrainerId && s.EndDate > DateTime.UtcNow , ct) ;

            if (hasActiveSessions) return false;

            var result = await _trainerRepo.DeleteAsync(trainer);

            return result > 0;
        }

     
    }
}
