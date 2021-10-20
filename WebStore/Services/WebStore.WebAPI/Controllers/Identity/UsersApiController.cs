using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(WebAPIAdresses.Identity.Users)]
    public class UsersApiController : ControllerBase
    {
        private readonly UserStore<User, Role, WebStoreDB> _UserStore;

        public UsersApiController(WebStoreDB db)
        {
            //IdentityUser
            //IdentityRole

            _UserStore = new UserStore<User, Role, WebStoreDB>(db);
            //_UserStore.AutoSaveChanges = false;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _UserStore.Users.ToArrayAsync();
        }
    }
}
