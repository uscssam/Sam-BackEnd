using Microsoft.AspNetCore.Mvc;
using SAM.Services.Interfaces;
using System.Net;

namespace SAM.Api.Controllers
{
    public class UserController : BaseController<User>
    {
        public UserController(IService<User> service) : base(service) { }

        public override IActionResult Create(User entity)
        {
            try
            {
                var created = service.Create(entity);
                if (created != null)
                    return Ok(new UserReturn(created));
                else return BadRequest();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public override IActionResult Get(int id)
        {
            var register = new UserReturn(service.Get(id));
            if (register != null)
                return Ok(register);
            else return NotFound(null);
        }

        public override IActionResult GetAll()
        {
            var registers = service.GetAll().Select(user => new UserReturn(user)).ToList();
            return Ok(registers);
        }

        public override IActionResult Update(User entity)
        {
            var updated = new UserReturn(service.Update(entity));
            if (updated != null)
                return Ok(updated);
            else return BadRequest();
        }
    }
}
