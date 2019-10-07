using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Administration;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _administratorRepository;

        public AdministratorService(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        public async Task<bool> AlreadyExistsAsync(Administrator administrator)
        {
            return await _administratorRepository.AlreadyExists(administrator).ConfigureAwait(false);
        }

        public async Task AddAsync(Administrator administrator)
        {
            await _administratorRepository.AddAsync(administrator).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Administrator administrator)
        {
            await _administratorRepository.UpdateAsync(administrator).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid administratorId)
        {
            await _administratorRepository.DeleteAsync(administratorId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Administrator>> GetAllAsync()
        {
            return await _administratorRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<Administrator> GetAsync(string username)
        {
            return await _administratorRepository.GetAsync(username).ConfigureAwait(false);
        }

        public async Task<Administrator> GetAsync(Guid administratorId)
        {
            return await _administratorRepository.GetAsync(administratorId).ConfigureAwait(false);
        }
    }
}
