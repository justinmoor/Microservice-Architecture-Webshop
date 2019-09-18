using System;

namespace Sprinters.SharedTypes.Authenticatie.Exceptions
{
    public class AccountCreationException : Exception
    {
        public AccountCreationException(string msg) : base(msg)
        {
        }
    }
}