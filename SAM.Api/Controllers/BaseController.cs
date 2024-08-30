﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAM.Entities;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public abstract class BaseController<T> : Controller
        where T : BaseEntity
    {
        protected readonly IService<T> service;

        public BaseController(IService<T> service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var register = service.Get(id);
            if (register != null)
                return Ok(register);
            else return NotFound(null);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(service.GetAll());
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Delete(int id)
        {
            var deleted = service.Delete(id);
            if (deleted)
                return Ok(null);
            else return NotFound(null);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Create(T entity)
        {
            var created = service.Create(entity);
            if (created != null)
                return Ok(created);
            else return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Update(T entity)
        {
            var updated = service.Update(entity);
            if(updated != null)
                return Ok(updated);
            else return BadRequest();
        }
    }
}
