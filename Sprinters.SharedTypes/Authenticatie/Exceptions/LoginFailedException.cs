using System;

namespace Sprinters.SharedTypes.Authenticatie.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(string msg) : base(msg)
        {
        }
    }
}