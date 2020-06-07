﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.ErrorCodes
{
    public enum ControllerErrorCode
    {
        InvalidUser,
        TodoItemIDInUse,
        CouldNotCreateItem,
        CouldNotCreateUser,
        PasswordsDoNotMatch,
        InvalidUserNameOrPassword,
        TokenIsExpeired,
        InvalidToken,
        CouldNotDeleteUser,
        PhoneNumberAlreadyExists,
        EmailAddressIsAlreadyTaken,
        CouldNotFindItem,
        NumberPlateIsAlreadyExists,
        TicketIsNotClosed,
        CouldNotDeleteTicket
    }
}
