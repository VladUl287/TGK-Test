using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.Database.Models;

namespace TestApi.Services.Contracts
{
    public interface ITokenService
    {
        string Generate(User user, string key, string issuer, string audience, DateTime expires);
    }
}
