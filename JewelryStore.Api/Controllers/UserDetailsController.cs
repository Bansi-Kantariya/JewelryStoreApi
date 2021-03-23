using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JewelryStore.Model;
using JewelryStore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JewelaryStore.Api
{
    [Route("[controller]")]
    [ApiController]
    public class UserDetailsController : Controller
    {
        IUserDetailRepository _userDetailRepository;

        public UserDetailsController(IUserDetailRepository userDetailRepository)
        {
            _userDetailRepository = userDetailRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("GetUser")]
        async public Task<ActionResult> GetUserDetails()
        {
            var response = await _userDetailRepository.GetUserDetails();

            if (response == null)
                return NotFound();
            else
                return Ok(response);
        }

        [HttpGet]
        [Route("AuthenticateUser")]
        async public Task<ActionResult> AuthenticateUser(string userName, string password)
        {
            var response = await _userDetailRepository.AuthenticateUser(userName, password);

            if (response == null)
                return BadRequest(response);
            else
                return Ok(response);
        }

        [HttpPost]
        [Route("AddUser")]
        async public Task<ActionResult> AddUser([FromBody]CustomeUserDetailsModelForInsert userDetails)
        {
            try
            {
                var response = await _userDetailRepository.InsertUserDetails(userDetails);

                if (response == 1)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
