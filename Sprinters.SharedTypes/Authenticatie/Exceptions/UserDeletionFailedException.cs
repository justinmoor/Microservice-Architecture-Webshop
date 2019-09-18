using System;

namespace Sprinters.SharedTypes.Authenticatie.Exceptions
{
    public class UserDeletionFailedException : Exception
    {
        public UserDeletionFailedException(string msg) : base(msg)
        {
        }
    }
}