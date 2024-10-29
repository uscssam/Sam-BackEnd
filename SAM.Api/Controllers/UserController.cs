using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAM.Services.Dto;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class UserController : BaseController<UserDto, UserSearchDto>
    {
        public UserController(IMapper mapper, IService<UserDto> service) : base(mapper, service) { }

        public override ActionResult<UserDto> Create(UserDto entity)
        {
            try
            {
                var created = service.Create(entity);
                if (created != null)
                    return Ok(new UserReturnDto(created));
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

        public override ActionResult<UserDto> Get(int id)
        {
            var register = new UserReturnDto(service.Get(id));
            if (register != null)
                return Ok(register);
            else return NotFound(null);
        }

        public override ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var registers = service.GetAll().Select(user => new UserReturnDto(user)).ToList();
            return Ok(registers);
        }

        public override ActionResult<UserDto> Update(int id, UserDto entity)
        {
            var updated = new UserReturnDto(service.Update(id, entity));
            if (updated != null)
                return Ok(updated);
            else return BadRequest();
        }
    }
}
