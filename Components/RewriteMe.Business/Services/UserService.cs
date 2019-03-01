using System;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool UserAlreadyExists(User user)
        {
            return _userRepository.UserAlreadyExists(user);
        }

        public void Add(User user)
        {
            _userRepository.Add(user);
        }

        public User GetUser(string username)
        {
            return _userRepository.GetUser(username);
        }

        public User GetUser(Guid userId)
        {
            return _userRepository.GetUser(userId);
        }
    }
}
