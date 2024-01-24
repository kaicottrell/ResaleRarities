using ApplicationCore.Models;
using ApplicationCore.Models.NonDbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IClientService
    {
        Task<ServiceResponse> RegisterUserAsync(ApplicationUser model);
        Task<ServiceResponse> LoginUserAsync(Login model);
    }
}
