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
    [Route(WebAPIAdresses.Identity.Roles)]
    public class RoleApiController : ControllerBase
    {
        private readonly RoleStore<Role> _RoleStore;

        public RoleApiController(WebStoreDB db)
        {
            _RoleStore = new (db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _RoleStore.Roles.ToArrayAsync();
        }
    }
}
