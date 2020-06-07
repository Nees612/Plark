using System;
using System.Collections.Generic;
using System.Text;

namespace Plark_MobileClient.Interface
{
    public interface IUserLoginView
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}
