﻿using System;

namespace Sprinters.SharedTypes.Authenticatie.Exceptions
{
    public class PasswordException : Exception
    {
        public PasswordException(string msg) : base(msg)
        {
        }
    }
}