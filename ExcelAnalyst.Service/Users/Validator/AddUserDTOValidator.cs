using ExcelAnalyst.Service.Objects.Users.DTOs;
using FluentValidation;

namespace ExcelAnalyst.Service.Objects.Users.Validator
{
    public class AddUserDTOValidator : AbstractValidator<AddUserDTO>
    {
        public AddUserDTOValidator()
        {
            RuleFor(u => u.UserName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
        }
    }
}
