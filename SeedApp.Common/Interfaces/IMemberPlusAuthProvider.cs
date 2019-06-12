using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeedApp.Common.Models;
using SeedApp.Common.Utilities;

namespace SeedApp.Common.Interfaces
{
    public interface IMemberPlusAuthProvider
    {
        Task<string> LoginAsyc(string username, string password);
    }
}