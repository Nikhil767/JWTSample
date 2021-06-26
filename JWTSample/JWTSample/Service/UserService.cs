using JWTSample.DataContext;
using JWTSample.Entities;
using JWTSample.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.Service
{
    public class UserService : IUserService
    {
        private readonly SampleDbContext _sampleDbContext;
        public UserService(SampleDbContext sampleDbContext)
        {
            _sampleDbContext = sampleDbContext;
        }
        public async Task<User> GetUserDetails(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("UserName is null");

            var currentUser = _sampleDbContext.Users.Local.FirstOrDefault(x => x.UserName.ToUpper() == userName.ToUpper());

            if (currentUser == null)
                throw new ArgumentException($"User with UserName : '{userName}' not found");

            return await Task.FromResult(currentUser);
        }
    }
}
