using Microsoft.VisualStudio.TestTools.UnitTesting;
using JewelryStore.Model;
using JewelryStore.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jewelrystore.RepositoryTest
{
    [TestClass]
    public class UserDetailRepositoryTests
    {
        DbContextOptionsBuilder<ApiContext> _optionsBuilder;
        ApiContext _dbContext;
        UserDetails _testUser;
        CustomeUserDetailsModelForInsert _testUserforInsert;
        CustomeUserDetailsModelForInsert _testUserforDuplicateInsert;

        [TestInitialize]
        public void GetUserDetailInitialize()
        {
            _optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            _optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            _dbContext = new ApiContext(_optionsBuilder.Options);

            SampleTestData.SetSampleTestData(_dbContext);

            Guid id = Guid.NewGuid();
            _testUser = new UserDetails
            {
                Id = id,
                UserName = "TestUser",
                Password = "TestUser".HashPassword(),
                UserType = 2
            };

            _testUserforInsert = new CustomeUserDetailsModelForInsert
            {
                UserName = "TestUserInsert",
                Password = "TestUserInsert",
                UserType = 2
            };

            _testUserforDuplicateInsert = new CustomeUserDetailsModelForInsert
            {
                UserName = "TestUserDuplicateInsert",
                Password = "TestUserDuplicateInsert",
                UserType = 2
            };

            _dbContext.Add(_testUser);
            _dbContext.SaveChanges();
        }

        [TestMethod]
        async public Task GetUserDetailTypeOfTest()
        {
            var _userDetailRepository = new UserDetailRepository(_dbContext);

            var response = await _userDetailRepository.GetUserDetails();

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(List<UserDetails>));
        }

        [TestMethod]
        async public Task GetUserDetailNotZeroTest()
        {
            var _userDetailRepository = new UserDetailRepository(_dbContext);

            var response = await _userDetailRepository.GetUserDetails();

            Assert.AreNotEqual(0, response.Count);
        }

        [TestMethod]
        async public Task AuthenticateUserCorrectDataTest()
        {
            

            var _userDetailRepository = new UserDetailRepository(_dbContext);

            var response = await _userDetailRepository.AuthenticateUser("TestUser", "TestUser".HashPassword());

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(CustomUserModel));
            Assert.AreEqual(_testUser.UserName, response.UserName);
            Assert.AreEqual(_testUser.UserType, response.UserType);
        }

        [TestMethod]
        async public Task AuthenticateUserInCorrectPasswordTest()
        {
            var _userDetailRepository = new UserDetailRepository(_dbContext);

            var response = await _userDetailRepository.AuthenticateUser("TestUser", "TestUser1".HashPassword());

            Assert.IsNull(response);
        }

        [TestMethod]
        async public Task AuthenticateUserInCorrectUserNameTest()
        {
            var _userDetailRepository = new UserDetailRepository(_dbContext);

            var response = await _userDetailRepository.AuthenticateUser("TestUser1", "TestUser".HashPassword());

            Assert.IsNull(response);
        }

        [TestMethod]
        async public Task InserUserWithCorrectDataTest()
        {
            var _userDetailRepository = new UserDetailRepository(_dbContext);

            var response = await _userDetailRepository.InsertUserDetails(_testUserforInsert);

            Assert.AreNotEqual(0, response);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),"Raise Exception when Dublicate insert is done")]
        async public Task InsertDuplicateDataTest()
        {
            var _userDetailRepository = new UserDetailRepository(_dbContext);

            var response = await _userDetailRepository.InsertUserDetails(_testUserforDuplicateInsert);

            var response2 = await _userDetailRepository.InsertUserDetails(_testUserforDuplicateInsert);

        }

        [TestCleanup]
        public void GetUserDetailsCleanUp()
        {
            _dbContext.Dispose();
        }

    }
}
