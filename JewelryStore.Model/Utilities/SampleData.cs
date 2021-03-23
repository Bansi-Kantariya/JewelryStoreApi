using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JewelryStore.Model
{
    public static class SampleData
    {

        public static void SetSampleData(IServiceProvider serviceProvider)
        {
            using (var context = new ApiContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApiContext>>()))
            {
                var user1 = new UserDetails
                {
                    Id = Guid.NewGuid(),
                    UserName = "NormalUser",
                    Password = "NormalUser".HashPassword(),
                    UserType = 1
                };

                context.UserDetails.Add(user1);

                var user2 = new UserDetails
                {
                    Id = Guid.NewGuid(),
                    UserName = "PriviledgedUser",
                    Password = "PriviledgedUser".HashPassword(),
                    UserType = 2
                };

                context.UserDetails.Add(user2);

                context.SaveChanges();
            }
        }
    }
}
