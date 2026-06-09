using Domain.Entities;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _repository
                .GetBySpecificationAsync(
                    Specification<T_User>.Eval(
                        x =>
                            x.Apo!.ToLower()
                            ==
                            request.Username.ToLower()),
                    q => q.Include(
                        x => x.UserRoleOrg));
        }
    }
}
