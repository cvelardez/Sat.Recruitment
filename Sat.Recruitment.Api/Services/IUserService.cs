using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
        Result CreateUser(User user);
    }
}
