using System;

namespace Sprinters.SharedTypes.Authenticatie.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(string msg) : base(msg)
        {
        }
    }
}