using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plark_Warden.Services
{
    public interface IScannerService
    {
        Task<string> ScanAsync();
    }
}
