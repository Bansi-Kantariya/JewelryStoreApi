using JewelryStore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JewelryStore.Repository
{
    public interface IUserDetailRepository
    {
        public Task<List<UserDetails>> GetUserDetails();
        public Task<CustomUserModel> AuthenticateUser(string UserName, string password);

        public Task<int> InsertUserDetails(CustomeUserDetailsModelForInsert userDetails);
    }
}
