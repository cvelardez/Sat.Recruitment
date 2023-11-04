using FluentValidation;
using Sat.Recruitment.Api.Models;
using System;

namespace Sat.Recruitment.Api.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(user => NormalizeEmail(user.Email))
                                       .EmailAddress()
                                       .WithMessage("The email address is not valid");
            RuleFor(user => user.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(user => user.Phone).NotEmpty().WithMessage("Phone is required");
        }

        private string NormalizeEmail(string email)
        {
            //Normalize email            
            if(!string.IsNullOrEmpty(email) && email.Contains('@'))
            {
                var aux = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

                var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

                aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

                email = string.Join("@", new string[] { aux[0], aux[1] });

                return email;
            }
            
            return null;
        }
    }
}
