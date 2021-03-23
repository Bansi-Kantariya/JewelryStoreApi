using AutoMapper;
using JewelryStore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryStore.Repository
{
    public class UserDetailRepository : IUserDetailRepository
    {
        ApiContext _apiContext;

        public UserDetailRepository(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        async public Task<List<UserDetails>> GetUserDetails()
        {
            return await _apiContext.UserDetails.ToListAsync();
        }

        async public Task<CustomUserModel> AuthenticateUser(string UserName, string password)
        {
            var user = await _apiContext.UserDetails
                        .Where(ud => ud.UserName == UserName
                            && ud.Password.ToUpper() == password.ToUpper())
                        .Select(s =>
                        new CustomUserModel
                        {
                            UserName = s.UserName,
                            UserType = s.UserType
                        }).Take(1).ToListAsync();

            return (user == null || user.Count == 0) ? null : user[0];
        }

        async public Task<int> InsertUserDetails(CustomeUserDetailsModelForInsert userDetails)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CustomeUserDetailsModelForInsert, UserDetails>());

            var mapper = config.CreateMapper();
            UserDetails userDetailsForInsert = mapper.Map<UserDetails>(userDetails);

            userDetailsForInsert.Id = Guid.NewGuid();
            userDetailsForInsert.Password = userDetailsForInsert.Password.HashPassword();

            _apiContext.UserDetails.Add(userDetailsForInsert);

            try
            {
                var response = await _apiContext.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
