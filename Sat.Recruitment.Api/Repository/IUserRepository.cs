using Sat.Recruitment.Api.Models;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Repository
{
    public interface IUserRepository
    {
        List<User> GetUsersFromDatabase();
        bool UserExists(User newUser, List<User> existingUsers);
        void Save(List<User> users);
    }
}
