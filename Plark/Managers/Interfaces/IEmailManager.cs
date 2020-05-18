using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Managers.Interfaces
{
    public interface IEmailManager
    {
        bool IsValidEmail(string email);
        public void SendVerificationEmail(string email, string emailToken);
        public long GenerateEmailId(long userId);
    }
}
