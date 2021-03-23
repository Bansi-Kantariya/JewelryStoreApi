using Microsoft.VisualStudio.TestTools.UnitTesting;
using JewelaryStore.Api;
using JewelryStore.Model;
using JewelryStore.Repository;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Jewelarystore.ApiTest
{
    [TestClass]
    public class UserDetailsControllerTest
    {
        Mock<IUserDetailRepository> _userDetailRepository;
        UserDetails _testUser1;
        List<UserDetails> _testUserList;
        CustomUserModel _testCustomeUserModel;
        CustomeUserDetailsModelForInsert _testUserforInsert;
        CustomeUserDetailsModelForInsert _testUserforDuplicateInsert;

        [TestInitialize]
        public void UserControllerTestInitialize()
        {
            _userDetailRepository = new Mock<IUserDetailRepository>();
            _testUser1 = new UserDetails
            {
                Id = Guid.NewGuid(),
                UserName = "TestUser1",
                Password = "TestUser1".HashPassword(),
                UserType = 1
            };

            _testUserList = new List<UserDetails>();
            _testUserList.Add(_testUser1);

            _testCustomeUserModel = new CustomUserModel
            {
                UserName = "TestUser",
                UserType = 1
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
        }


        [TestMethod]
        async public Task GetUserDetailsNotFoundTest()
        {
            _userDetailRepository.Setup(x => x.GetUserDetails()).ReturnsAsync(() => null);

            UserDetailsController controller = new UserDetailsController(_userDetailRepository.Object);

            ActionResult response = await controller.GetUserDetails();

            Assert.IsInstanceOfType(response, typeof(NotFoundResult));

        }

        [TestMethod]
        async public Task GetUserDetailsResultFoundTest()
        {
            _userDetailRepository.Setup(x => x.GetUserDetails()).ReturnsAsync(() => _testUserList);

            UserDetailsController controller = new UserDetailsController(_userDetailRepository.Object);

            ActionResult response = await controller.GetUserDetails();
            var okResponse = response as OkObjectResult;

            var responseValue = okResponse.Value as List<UserDetails>;


            Assert.IsNotNull(okResponse);
            Assert.IsNotNull(okResponse.Value);
            Assert.IsNotNull(responseValue);
            Assert.AreEqual(_testUser1.Id, responseValue[0].Id);

        }

        [TestMethod]
        async public Task AuthenticateUserBadRequestTest()
        {
            _userDetailRepository.Setup(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            UserDetailsController controller = new UserDetailsController(_userDetailRepository.Object);

            ActionResult response = await controller.AuthenticateUser("testuser", "testUser1".HashPassword());

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));

        }

        [TestMethod]
        async public Task AuthenticateUserResultFoundTest()
        {
            _userDetailRepository.Setup(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => _testCustomeUserModel);

            UserDetailsController controller = new UserDetailsController(_userDetailRepository.Object);

            ActionResult response = await controller.AuthenticateUser("TestUser", "TestUser".HashPassword());
            var okResponse = response as OkObjectResult;

            var responseValue = okResponse.Value as CustomUserModel;


            Assert.IsNotNull(okResponse);
            Assert.IsNotNull(okResponse.Value);
            Assert.IsNotNull(responseValue);
            Assert.AreEqual("TestUser", responseValue.UserName);
            Assert.AreEqual(1, responseValue.UserType);

        }

        [TestMethod]
        async public Task AddUserCorrectDataTest()
        {
            _userDetailRepository.Setup(x => x.InsertUserDetails(It.IsAny<CustomeUserDetailsModelForInsert>()))
                .ReturnsAsync(() => 1);

            UserDetailsController controller = new UserDetailsController(_userDetailRepository.Object);

            ActionResult response = await controller.AddUser(_testUserforInsert);
            var okResponse = response as OkObjectResult;

            var responseValue = okResponse.Value as int?;

            Assert.IsNotNull(okResponse);
            Assert.IsNotNull(okResponse.Value);
            Assert.IsNotNull(responseValue);
            Assert.AreEqual(1, responseValue);

        }

        [TestMethod]
        async public Task AddUserDuplicateDataTest()
        {
            _userDetailRepository.Setup(x => x.InsertUserDetails(It.IsAny<CustomeUserDetailsModelForInsert>()))
                .Throws(new InvalidOperationException());

            UserDetailsController controller = new UserDetailsController(_userDetailRepository.Object);

            ActionResult response = await controller.AddUser(_testUserforDuplicateInsert);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));

        }

    }
}
