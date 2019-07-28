﻿using System;
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

        public async Task AddAsync(Administrator administrator)
        {
            await _administratorRepository.AddAsync(administrator).ConfigureAwait(false);
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