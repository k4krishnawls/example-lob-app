using System;
using System.Threading.Tasks;

namespace ELA.Common.Authentication
{
    public interface ISignInManager
    {
        Task<SignInResult> SignInAsync(string UserName, string Password);
    }
}
