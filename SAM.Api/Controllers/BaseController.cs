using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAM.Services.Dto;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public abstract class BaseController<T, TSearch> : ControllerBase
        where T : BaseDto
        where TSearch : BaseDto
    {
        protected readonly IMapper mapper;
        protected readonly IService<T> service;

        protected BaseController(IMapper mapper, IService<T> service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual ActionResult<T> Get(int id)
        {
            var register = service.Get(id);
            if (register != null)
                return Ok(register);
            else return NotFound(null);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual ActionResult<IEnumerable<T>> GetAll()
        {
            return Ok(service.GetAll());
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual ActionResult<IEnumerable<T>> Search(TSearch entity)
        {
            return Ok(service.Search(mapper.Map<T>(entity)));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual ActionResult Delete([FromRoute] int id)
        {
            try
            {
                var deleted = service.Delete(id);
                if (deleted)
                    return Ok(true);
                else return NotFound(null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual ActionResult<T> Create(T entity)
        {
            var created = service.Create(entity);
            if (created != null)
                return Ok(created);
            else return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public virtual ActionResult<T> Update(int id, T entity)
        {
            var updated = service.Update(id, entity);
            if(updated != null)
                return Ok(updated);
            else return BadRequest();
        }
    }
}
