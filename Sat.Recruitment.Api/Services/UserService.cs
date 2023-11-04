using FluentValidation;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository userRepository, IValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
        }

        public Result CreateUser(User user)
        {
            var validationResult = _userValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return new Result { IsSuccess = false, Errors = errors };
            }

            ProcessUserMoney(user);

            var userList = _userRepository.GetUsersFromDatabase();
            if(!_userRepository.UserExists(user, userList))
            {
                userList.Add(user);
                _userRepository.Save(userList);

                return new Result { IsSuccess = true, Errors = null };
            }

            return new Result { IsSuccess = false, Errors = new List<string> { "User already Exists" } };
        }

        private void ProcessUserMoney(User user)
        {
            switch (user.UserType)
            {
                default:
                case UserType.Normal:
                    CalculateNormalUserMoney(user);
                    break;
                case UserType.SuperUser:
                    CalculateSuperUserMoney(user);
                    break;
                case UserType.Premium:
                    CalculatePremiumUserMoney(user);
                    break;
            }
        }
        private void CalculateNormalUserMoney(User user)
        {
            decimal money = user.Money;

            if (money > 100)
            {
                user.Money = money + money * 0.12M;
            }
            else if (money > 10)
            {
                user.Money = money + money * 0.08M;
            }
        }

        private void CalculateSuperUserMoney(User user)
        {
            decimal money = user.Money;
            user.Money = money > 100 ? money + money * 0.2M : user.Money;
        }

        private void CalculatePremiumUserMoney(User user)
        {
            decimal money = user.Money;

            if (money >100) 
                user.Money = money + money * 2;
        }
    }
}
