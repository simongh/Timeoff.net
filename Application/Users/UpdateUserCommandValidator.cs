﻿using FluentValidation;

namespace Timeoff.Application.Users
{
    internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(
            UserDetailsBaseValidator baseValidator)
        {
            Include(baseValidator);
        }
    }
}