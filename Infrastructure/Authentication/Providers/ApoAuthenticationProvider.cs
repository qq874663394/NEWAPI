using Domain.Interface.Repositories;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Authentication.Providers
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
            return _repository.Query(p => p.Apo!.ToLower()==request.Username.ToLower()).Include(x => x.UserRoleOrg);
        }
    }
}
