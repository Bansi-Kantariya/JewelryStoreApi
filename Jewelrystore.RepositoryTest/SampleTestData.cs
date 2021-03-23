using JewelryStore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jewelrystore.RepositoryTest
{
    public static class SampleTestData
    {
        public static void SetSampleTestData(ApiContext context)
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
