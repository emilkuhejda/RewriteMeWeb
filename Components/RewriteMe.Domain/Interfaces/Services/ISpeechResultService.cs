﻿using System.Threading.Tasks;
using RewriteMe.Domain.Recording;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ISpeechResultService
    {
        Task AddAsync(SpeechResult speechResult);
    }
}
