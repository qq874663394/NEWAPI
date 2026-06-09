using Domain.Entities;
using Domain.Interface.IServices.Authentication;
using Domain.Model.Authentication;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication.Providers
{
    public class PasswordAuthenticationProvider
           : IAuthenticationProvider
    {
        private readonly IRepository<T_User> _repository;

        public string Key => "Password";

        public PasswordAuthenticationProvider(
            IRepository<T_User> repository)
        {
            _repository = repository;
        }

        public async Task<T_User?> AuthenticateAsync(
            AuthenticationRequest request)
        {
            var user =
                (await _repository.GetAllAsync(
                    Specification<T_User>.Eval(
                        x =>
                            x.Name!.ToLower()
                            ==
                            request.Username.ToLower()),
                    q => q.Include(
                        x => x.UserRoleOrg)))
                .FirstOrDefault();

            if (user == null)
                return null;

            if (!CryptHelper.VerifyPassword(
                    request.Password,
                    user.Password))
            {
                return null;
            }

            user.LastLoginTime = DateTime.Now;

            return user;
        }
    }
}
