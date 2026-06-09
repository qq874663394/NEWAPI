using Domain.Entities;
using Domain.Interface.Repositories;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Authentication.Providers
{
    public class ApoAuthenticationProvider
           : IAuthenticationProvider
    {
        private readonly IRepository<T_User> _repository;

        public string Key => "Apo";

        public ApoAuthenticationProvider(
            IRepository<T_User> repository)
        {
            _repository = repository;
        }

        public async Task<T_User?> AuthenticateAsync(
            AuthenticationRequest request)
        {
            return await _repository.Query(
                        x =>x.Apo!.ToLower() == request.Username.ToLower())
                .Include(x => x.UserRole);
        }
    }
}
